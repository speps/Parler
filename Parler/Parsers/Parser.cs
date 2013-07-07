using System;
using System.Collections.Generic;
using System.Linq;

namespace Parler.Parsers
{
    public delegate void OnChainResult(IResult result);

    public abstract class Parser : IEquatable<Parser>
    {
        public abstract IEnumerable<IResult> Apply(Input input);
        public abstract void Chain(Trampoline t, Input input, OnChainResult result);

        internal abstract ISet<char?> ComputeFirst(ISet<Parser> seen);

        public ISet<char> First()
        {
            var set = ComputeFirst(new HashSet<Parser>());
            if (set == null)
            {
                set = new HashSet<char?>();
            }

            if (set.Contains(null))
            {
                return Misc.UniversalCharSet;
            }

            return new HashSet<char>(set.Select(c => c.Value));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Parser);
        }

        public static bool operator ==(Parser lhs, Parser rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Parser lhs, Parser rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        public bool Equals(Parser other)
        {
            if (other == null)
            {
                return false;
            }
            return object.ReferenceEquals(this, other);
        }
    }
}