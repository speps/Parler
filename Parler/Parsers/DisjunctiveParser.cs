using System;
using System.Collections.Generic;
using System.Linq;

namespace Parler.Parsers
{
    internal class ThunkParser : NonTerminalParser
    {
        private Parser OtherParser;
        private Action<Trampoline, Input, OnChainResult> ChainDelegate;

        public ThunkParser(Parser parser, Action<Trampoline, Input, OnChainResult> chainDelegate)
        {
            OtherParser = parser;
            ChainDelegate = chainDelegate;
        }

        internal override ISet<char?> ComputeFirst(ISet<Parser> seen)
        {
            return OtherParser.ComputeFirst(seen);
        }

        public override void Chain(Trampoline t, Input input, OnChainResult result)
        {
            ChainDelegate(t, input, result);
        }

        public override bool Equals(object obj)
        {
            return OtherParser.Equals(obj);
        }

        public override int GetHashCode()
        {
            return OtherParser.GetHashCode();
        }
    }

    public class DisjunctiveParser : NonTerminalParser
    {
        private Parser Left;
        private Parser Right;

        public DisjunctiveParser(Parser left, Parser right)
        {
            Left = left;
            Right = right;
        }

        private IEnumerable<Parser> Gather()
        {
            return GatherImpl(new HashSet<DisjunctiveParser>());
        }

        private bool IsLL1()
        {
            var sets = Gather().Select(p => p.First());
            var areFinite = sets.All(s => !s.IsComplement());

            if (areFinite)
            {
                var totalSize = sets.Aggregate(0, (count, s) => count + s.Count);
                var union = sets.Aggregate(new HashSet<char>(), (u, s) => { u.UnionWith(s); return u; });
                return totalSize == union.Count && sets.All(s => s.Count > 0);
            }
            return false;
        }

        internal override ISet<char?> ComputeFirst(ISet<Parser> seen)
        {
            if (seen.Contains(this))
            {
                return null;
            }
            else
            {
                var newSeen = new HashSet<Parser>(seen);
                newSeen.Add(this);

                var gather = GatherImpl(new HashSet<DisjunctiveParser>());
                var firstSets = gather.Select(p => p.ComputeFirst(newSeen)).Where(f => f != null).ToList();
                firstSets.Sort((x, y) => true.CompareTo((x.IsComplement() || !y.IsComplement())));
                var back = firstSets.Aggregate(new HashSet<char?>(), (u, s) => { u.UnionWith(s); return u; });
                return back;
            }
        }

        private IDictionary<char, Parser> Predict()
        {
            return Gather().Aggregate(new Dictionary<char, Parser>(), (map, p) =>
            {
                foreach (var c in p.First())
                {
                    map[c] = p;
                }
                return map;
            });
        }

        public override void Chain(Trampoline t, Input input, OnChainResult result)
        {
            if (IsLL1())
            {
                if (input.EOF)
                {
                    result(Result.Failure(input, FailureType.UnexpectedEndOfStream));
                }
                else
                {
                    var predict = Predict();
                    Parser parser;
                    if (predict.TryGetValue(input.Current, out parser))
                    {
                        parser.Chain(t, input, result);
                    }
                    else
                    {
                        result(Result.Failure(input, FailureType.UnexpectedChars));
                    }
                }
            }
            else
            {
                var thunk = new ThunkParser(this, (tr, i, onResult) =>
                {
                    var results = new HashSet<IResult>();

                    var predicted = false;
                    var gather = Gather();
                    foreach (var p in gather)
                    {
                        if ((!i.EOF || p.First() == Misc.UniversalCharSet)
                            || (i.EOF || p.First().Contains(i.Current)))
                        {
                            predicted = true;
                            tr.Add(p, i, r =>
                            {
                                if (!results.Contains(r))
                                {
                                    Misc.Trace("Reduced: {0} *=> {1}", this, r);
                                    onResult(r);
                                    results.Add(r);
                                }
                            });
                        }
                    }

                    if (!predicted)
                    {
                        if (i.EOF)
                        {
                            onResult(Result.Failure(i, FailureType.UnexpectedEndOfStream));
                        }
                        else
                        {
                            onResult(Result.Failure(i, FailureType.UnexpectedChars));
                        }
                    }
                });

                t.Add(thunk, input, result);
            }
        }

        protected virtual IList<Parser> GatherImpl(ISet<DisjunctiveParser> seen)
        {
            var newSeen = new HashSet<DisjunctiveParser>(seen);
            newSeen.Add(this);

            Func<Parser, IList<Parser>> process = p =>
            {
                if (p is DisjunctiveParser)
                {
                    var d = p as DisjunctiveParser;
                    if (!seen.Contains(d))
                    {
                        return d.GatherImpl(newSeen);
                    }
                    else
                    {
                        return new List<Parser>();
                    }
                }
                else
                {
                    var r = new List<Parser>();
                    r.Add(p);
                    return r;
                }
            };

            var result = new List<Parser>();
            result.AddRange(process(Left));
            result.AddRange(process(Right));
            return result;
        }
    }
}
