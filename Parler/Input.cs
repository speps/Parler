using System;
using System.Collections.Generic;

namespace Parler
{
    public class Input : IEquatable<Input>
    {
        public string Source { get; private set; }
        public int Position { get; private set; }

        public char Current
        {
            get { return Source[Position]; }
        }

        public bool EOF
        {
            get { return Source.Length == Position; }
        }

        public Input(string source)
        {
            Source = source;
            Position = 0;
        }

        public Input Advance()
        {
            return new Input(Source) { Position = Position + 1 };
        }

        public string Take(int count)
        {
            var chars = new List<char>();
            int index = Position;
            for (int i = 0; i < count && i < (Source.Length - Position); i++)
            {
                chars.Add(Source[Position + i]);
                index++;
            }
            return new string(chars.ToArray());
        }

        public Input Drop(int count)
        {
            var result = this;
            for (int i = 0; i < count && !result.EOF; i++)
            {
                result = result.Advance();
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Input);
        }

        public static bool operator ==(Input lhs, Input rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Input lhs, Input rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Source, Position);
        }

        public override string ToString()
        {
            return string.Format("Input({0}, {1})", Position, Source.Length - 1);
        }

        /// <remarks>
        /// Only Position is used, Source will likely be the same between 2 Input
        /// because only one Input can be given to a parser.
        /// </remarks>
        public bool Equals(Input other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return Position == other.Position;
        }
    }
}
