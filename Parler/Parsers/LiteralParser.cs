using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parler.Parsers
{
    public class LiteralParser : TerminalParser
    {
        public string Literal;

        public LiteralParser(string literal)
        {
            Literal = literal;
        }

        internal override ISet<char?> ComputeFirst(ISet<Parser> seen)
        {
            if (Literal.Length > 0)
            {
                return new HashSet<char?>(new char?[] { Literal[0] });
            }
            else
            {
                return new HashSet<char?>(new char?[] { null });
            }
        }

        public override IResult Parse(Input input)
        {
            var trunc = input.Take(Literal.Length);

            if (trunc.Length != Literal.Length)
            {
                return Result.Failure(input, FailureType.UnexpectedEndOfStream);
            }
            else
            {
                if (trunc == Literal)
                {
                    return Result.Success(input.Drop(Literal.Length), Literal);
                }
                else
                {
                    return Result.Failure(input, FailureType.ExpectedLiteral);
                }
            }
        }
    }
}
