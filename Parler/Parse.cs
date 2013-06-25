using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Parler
{
    //public class Wrapper<T>
    //{
    //    public Func<Parser<T>> ParserFunc;
    //    public string Name;

    //    public Wrapper(Expression<Func<Parser<T>>> expr)
    //    {
    //        if (expr.NodeType != ExpressionType.Lambda)
    //            throw new ArgumentException("Expression is not a lambda.", "expr");
    //        if (expr.Body.NodeType != ExpressionType.MemberAccess)
    //            throw new ArgumentException("Expression body is not a member/variable access.", "expr");
    //        ParserFunc = expr.Compile();
    //        Name = (expr.Body as MemberExpression).Member.Name;
    //    }
    //}

    //public static class Parse
    //{
    //    public static Parser<char> Chars(string chars)
    //    {
    //        return new CharsParser(chars);
    //    }

    //    public static Parser<T> Ref<T>(Expression<Func<Parser<T>>> p)
    //    {
    //        return new RefParser<T>(p);
    //    }

    //    public static Parser<T> Return<T>(T value)
    //    {
    //        return new ReturnParser<T>(value);
    //    }

    //    public static IResult<T> TryParse<T>(this Parser<T> parser, string input)
    //    {
    //        return parser.Apply(new Input { Source = input });
    //    }
    //}
}
