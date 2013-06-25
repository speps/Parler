using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parler
{
    public class Input
    {
        public string Source;
        public int Position;

        public char Current
        {
            get { return Source[Position]; }
        }

        public bool EOF
        {
            get { return Source.Length == Position; }
        }

        public Input Advance()
        {
            return new Input { Source = Source, Position = Position + 1 };
        }

        public override bool Equals(object obj)
        {
            var other = obj as Input;
            return Source == other.Source && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Source, Position);
        }
    }
}
