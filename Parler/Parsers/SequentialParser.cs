using System;
using System.Collections.Generic;

namespace Parler.Parsers
{
    public class SequentialParser : NonTerminalParser
    {
        private Parser Left;
        private Parser Right;
        private Func<object, object, object> Combinator;

        public SequentialParser(Parser left, Parser right, Func<object, object, object> combinator)
        {
            Left = left;
            Right = right;
            Combinator = combinator;
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
                var setLeft = Left.ComputeFirst(newSeen);

                if (setLeft != null)
                {
                    if (setLeft.Count == 0 || setLeft.Contains(null))
                    {
                        var setRight = Right.ComputeFirst(newSeen);
                        if (setRight != null)
                        {
                            if (setLeft.IsComplement())
                            {
                                var newSet = new ComplementarySet<char?>(setLeft as ComplementarySet<char?>);
                                newSet.Remove(null);
                                newSet.UnionWith(setRight);
                                return newSet;
                            }
                            else
                            {
                                var newSetLeft = new HashSet<char?>(setLeft);
                                newSetLeft.Remove(null);
                                var newSetRight = new HashSet<char?>(setRight);
                                newSetRight.UnionWith(newSetLeft);
                                return newSetRight;
                            }
                        }
                    }
                }

                return setLeft;
            }
        }

        public override void Chain(Trampoline t, Input input, OnChainResult result)
        {
            Left.Chain(t, input, resLeft =>
            {
                if (resLeft is ISuccess)
                {
                    Right.Chain(t, resLeft.Remainder, resRight =>
                    {
                        if (resRight is ISuccess)
                        {
                            var sucLeft = resLeft as ISuccess;
                            var sucRight = resRight as ISuccess;
                            result(Result.Success(resRight.Remainder, Combinator(sucLeft.Value, sucRight.Value)));
                        }
                        else
                        {
                            result(resRight);
                        }
                    });
                }
                else
                {
                    result(resLeft);
                }
            });
        }

        public override bool Equals(object obj)
        {
            var other = obj as SequentialParser;
            if (other == null)
                return false;
            return Left.Equals(other.Left) && Right.Equals(other.Right);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Left, Right);
        }
    }
}
