using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Parler
{
    public interface IParser
    {
        IResult Apply(Input input);
        IResult Chain(Trampoline t, Input input);
    }

    public abstract class Parser<T> : IParser
    {
        public string Name { get; private set; }

        public Parser(string name)
        {
            Name = name;
        }

        IResult IParser.Apply(Input input) { return Apply(input); }
        IResult IParser.Chain(Trampoline t, Input input) { return Chain(t, input); }

        public abstract IResult<T> Apply(Input input);
        public abstract IResult<T> Chain(Trampoline t, Input input);
    }

    public abstract class TerminalParser<T> : Parser<T>
    {
        public TerminalParser(string name)
            : base(name)
        {

        }

        public override abstract IResult<T> Apply(Input input);

        public override sealed IResult<T> Chain(Trampoline t, Input input)
        {
            return Apply(input);
        }
    }

    public abstract class NonTerminalParser<T> : Parser<T>
    {
        public NonTerminalParser(string name)
            : base(name)
        {

        }

        public override abstract IResult<T> Apply(Input input);

        public override sealed IResult<T> Chain(Trampoline t, Input input)
        {
            return Apply(input);
        }
    }

    public class DisjunctiveParser<T> : NonTerminalParser<T>
    {
        public Parser<T> Left { get; private set; }
        public Parser<T> Right { get; private set; }

        public DisjunctiveParser(Parser<T> left, Parser<T> right)
            : base(string.Concat(left.Name, " | ", right.Name))
        {
            Left = left;
            Right = right;
        }

        public override IResult<T> Apply(Input input)
        {
            var result = Left.Apply(input);
            if (result is ISuccess)
            {
                return result;
            }
            return Right.Apply(input);
        }
    }

    public class SequentialParser<T> : NonTerminalParser<T>
    {
        public Parser<T> Left { get; private set; }
        public Parser<T> Right { get; private set; }

        public SequentialParser(Parser<T> left, Parser<T> right)
            : base(string.Concat(left.Name, " ~ ", right.Name))
        {
            Left = left;
            Right = right;
        }

        public override IResult<T> Apply(Input input)
        {
            var result = Left.Apply(input);
            if (result is ISuccess)
            {
                return Right.Apply(result.Remainder);
            }
            return Result.Failure<T>(input);
        }
    }

    public class CharsParser : TerminalParser<char>
    {
        public string Chars { get; private set; }

        public CharsParser(string chars)
            : base(chars)
        {
            Chars = chars;
        }

        public override IResult<char> Apply(Input input)
        {
            if (!input.EOF && Chars.Contains(input.Current))
            {
                return Result.Success<char>(input.Advance(), input.Current);
            }
            return Result.Failure<char>(input);
        }
    }

    public class RefParser<T> : NonTerminalParser<T>
    {
        public Expression<Func<Parser<T>>> Expr;
        public Func<Parser<T>> ParserFunc;

        public RefParser(Expression<Func<Parser<T>>> expr)
            : base(expr.Name())
        {
            Expr = expr;
            ParserFunc = Expr.Compile();
        }

        public override IResult<T> Apply(Input input)
        {
            return ParserFunc().Apply(input);
        }
    }
}
