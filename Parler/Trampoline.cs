using Parler.Parsers;
using System;
using System.Collections.Generic;

namespace Parler
{
    public class Trampoline
    {
        class QueueTuple
        {
            public Parser Parser;
            public Input Input;
        }

        private Stack<QueueTuple> Queue = new Stack<QueueTuple>();
        private IDictionary<Input, ISet<Parser>> Done = new Dictionary<Input, ISet<Parser>>();
        private IDictionary<Input, IDictionary<Parser, HashSet<ISuccess>>> Popped = new Dictionary<Input, IDictionary<Parser, HashSet<ISuccess>>>();
        private IDictionary<Input, IDictionary<Parser, HashSet<OnChainResult>>> BackLinks = new Dictionary<Input, IDictionary<Parser, HashSet<OnChainResult>>>();
        private IDictionary<IResult, HashSet<OnChainResult>> Saved = new Dictionary<IResult, HashSet<OnChainResult>>();

        public bool HasNext { get { return Queue.Count > 0; } }

        public void Step()
        {
            var tuple = Remove();
            tuple.Parser.Chain(this, tuple.Input, result =>
            {
                Popped.GetOrAdd(tuple.Input,
                    parsers =>
                    {
                        if (!parsers.ContainsKey(tuple.Parser))
                        {
                            parsers.Add(tuple.Parser, new HashSet<ISuccess>());
                        }
                    },
                    () =>
                    {
                        var parsers = new Dictionary<Parser, HashSet<ISuccess>>();
                        parsers.Add(tuple.Parser, new HashSet<ISuccess>());
                        return parsers;
                    }
                );

                if (result is ISuccess)
                {
                    Popped[tuple.Input][tuple.Parser].Add(result as ISuccess);
                    Misc.Trace("Saved: {0} *=> {1}", tuple, result);
                }

                Saved.GetOrAdd(result,
                    set =>
                    {
                        var fset = BackLinks[tuple.Input][tuple.Parser];
                        foreach (var f in fset)
                        {
                            if (!set.Contains(f))
                            {
                                set.Add(f);
                                f(result);
                            }
                        }
                    },
                    () =>
                    {
                        var set = new HashSet<OnChainResult>();

                        var fset = BackLinks[tuple.Input][tuple.Parser];
                        foreach (var f in fset)
                        {
                            set.Add(f);
                            f(result);
                        }

                        return set;
                    }
                );
            });
        }

        public void Add(Parser parser, Input input, OnChainResult continuation)
        {
            var tuple = new QueueTuple { Parser = parser, Input = input };

            BackLinks.GetOrAdd(input,
                parsers =>
                {
                    if (!parsers.ContainsKey(parser))
                    {
                        parsers.Add(parser, new HashSet<OnChainResult>());
                    }
                },
                () =>
                {
                    var parsers = new Dictionary<Parser, HashSet<OnChainResult>>();
                    parsers.Add(parser, new HashSet<OnChainResult>());
                    return parsers;
                }
            );

            BackLinks[input][parser].Add(continuation);

            Popped.Get(input,
                parsers =>
                {
                    if (parsers.ContainsKey(parser))
                    {
                        foreach (var result in parsers[parser])
                        {
                            Misc.Trace("Revisited: {0} *=> {1}", tuple, result);
                            continuation(result);
                        }
                        return true;
                    }
                    return false;
                },
                () =>
                {
                    Done.GetOrAdd(input,
                        parsers =>
                        {
                            if (!parsers.Contains(parser))
                            {
                                AddTuple(parsers, tuple);
                            }
                        },
                        () =>
                        {
                            var parsers = new HashSet<Parser>();
                            AddTuple(parsers, tuple);
                            return parsers;
                        }
                    );
                }
            );
        }

        private void AddTuple(ISet<Parser> parsers, QueueTuple tuple)
        {
            Queue.Push(tuple);
            parsers.Add(tuple.Parser);
        }

        private QueueTuple Remove()
        {
            var tuple = Queue.Pop();
            return tuple;
        }
    }
}
