using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parler
{
    public class Trampoline
    {
        class SSet : HashSet<ISuccess>
        {

        }

        class FSet : HashSet<Action<IResult>>
        {

        }

        struct QueueTuple
        {
            public IParser Parser;
            public Input Input;
        }

        private Stack<QueueTuple> Queue = new Stack<QueueTuple>();
        private Dictionary<Input, HashSet<IParser>> Done = new Dictionary<Input, HashSet<IParser>>();
        private Dictionary<Input, Dictionary<IParser, SSet>> Popped = new Dictionary<Input, Dictionary<IParser, SSet>>();
        private Dictionary<Input, Dictionary<IParser, FSet>> BackLinks = new Dictionary<Input, Dictionary<IParser, FSet>>();
        private Dictionary<IResult, FSet> Saved = new Dictionary<IResult, FSet>();

        public bool HasNext { get { return Queue.Count > 0; } }

        public IResult Step()
        {
            var tuple = Remove();
            var result = tuple.Parser.Chain(this, tuple.Input);

            Popped.GetOrAdd(tuple.Input,
                parsers => {
                    if (!parsers.ContainsKey(tuple.Parser))
                    {
                        parsers.Add(tuple.Parser, new SSet());
                    }
                },
                () => {
                    var parsers = new Dictionary<IParser, SSet>();
                    parsers.Add(tuple.Parser, new SSet());
                    return parsers;
                }
            );

            if (result is ISuccess)
            {
                Popped[tuple.Input][tuple.Parser].Add(result as ISuccess);
            }

            Saved.GetOrAdd(result,
                set => {
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
                () => {
                    var set = new FSet();
                    
                    var fset = BackLinks[tuple.Input][tuple.Parser];
                    foreach (var f in fset)
                    {
                        set.Add(f);
                        f(result);
                    }

                    return set;
                }
            );

            return result;
        }

        public void Add(IParser parser, Input input, Action<IResult> continuation)
        {
            var tuple = new QueueTuple { Parser = parser, Input = input };

            BackLinks.GetOrAdd(input,
                parsers => {
                    if (!parsers.ContainsKey(parser))
                    {
                        parsers.Add(parser, new FSet());
                    }
                },
                () => {
                    var parsers = new Dictionary<IParser, FSet>();
                    parsers.Add(parser, new FSet());
                    return parsers;
                }
            );

            BackLinks[input][parser].Add(continuation);

            Popped.Get(input,
                parsers => {
                    if (parsers.ContainsKey(parser))
                    {
                        foreach (var result in parsers[parser])
                        {
                            continuation(result);
                        }
                        return true;
                    }
                    return false;
                },
                () => {
                    Done.GetOrAdd(input,
                        parsers => {
                            if (!parsers.Contains(parser))
                            {
                                AddTuple(parsers, tuple);
                            }
                        },
                        () => {
                            var parsers = new HashSet<IParser>();
                            AddTuple(parsers, tuple);
                            return parsers;
                        }
                    );
                }
            );
        }

        private void AddTuple(HashSet<IParser> parsers, QueueTuple tuple)
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
