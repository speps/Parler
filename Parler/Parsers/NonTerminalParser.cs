using System;
using System.Collections.Generic;
using System.Linq;

namespace Parler.Parsers
{
    public abstract class NonTerminalParser : Parser
    {
        public override IEnumerable<IResult> Apply(Input input)
        {
            var t = new Trampoline();

            var successes = new HashSet<ISuccess>();
            var failures = new HashSet<IFailure>();

            bool recognized = false;

            Func<IEnumerable<IResult>> parse = null;
            parse = () =>
            {
                if (t.HasNext)
                {
                    t.Step();

                    if (successes.Count == 0)
                    {
                        return parse();
                    }
                    else
                    {
                        var results = successes.ToList<IResult>();
                        successes.Clear();
                        results.AddRange(parse());
                        return results;
                    }
                }
                else
                {
                    if (recognized)
                    {
                        return successes.ToList<IResult>();
                    }
                    else
                    {
                        return failures.ToList<IResult>();
                    }
                }
            };

            Chain(t, input, result =>
            {
                if (result is ISuccess)
                {
                    Misc.Trace("Top-Level Success: {0}", result);
                    if (result.Remainder.EOF)
                    {
                        Misc.Trace("Tail Accepted: {0}", result);
                        recognized = true;
                        successes.Add(result as ISuccess);
                    }
                    else
                    {
                        Misc.Trace("Tail Rejected: {0}", result);
                        failures.Add(Result.Failure(result.Remainder, FailureType.UnexpectedTrailingChars));
                    }
                }
                else
                {
                    Misc.Trace("Top-Level Failure: {0}", result);
                    failures.Add(result as IFailure);
                }
            });

            return parse();
        }
    }
}