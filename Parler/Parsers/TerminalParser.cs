using System.Collections.Generic;

namespace Parler.Parsers
{
    public abstract class TerminalParser : Parser
    {
        public override IEnumerable<IResult> Apply(Input input)
        {
            var result = Parse(input);
            if (result is ISuccess)
            {
                if (result.Remainder.EOF)
                {
                    return new IResult[] { result };
                }
                else
                {
                    return new IResult[] { Result.Failure(result.Remainder, FailureType.UnexpectedTrailingChars) };
                }
            }
            return new IResult[] { result };
        }

        public override void Chain(Trampoline t, Input input, OnChainResult result)
        {
            result(Parse(input));
        }

        public abstract IResult Parse(Input input);
    }
}