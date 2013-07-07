using System;

namespace Parler
{
    public interface IResult
    {
        Input Remainder { get; }
    }

    public interface ISuccess : IResult
    {
        object Value { get; }
    }

    public class Success : ISuccess, IEquatable<ISuccess>
    {
        public Input Remainder { get; internal set; }
        public object Value { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ISuccess);
        }

        public static bool operator ==(Success lhs, ISuccess rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Success lhs, ISuccess rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Remainder, Value);
        }

        public override string ToString()
        {
            return string.Format("Success({0}, {1})", Remainder, Value);
        }

        public bool Equals(ISuccess other)
        {
            if (object.ReferenceEquals(other, null))
                return false;
            if (Value == null && other.Value != null)
                return false;
            if (Value != null && other.Value == null)
                return false;
            return Remainder.Equals(other.Remainder) && Value.Equals(other.Value);
        }
    }

    public interface IFailure : IResult
    {
    }

    public class Failure : IFailure, IEquatable<IFailure>
    {
        public Input Remainder { get; internal set; }
        public FailureType Type { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as IFailure);
        }

        public static bool operator ==(Failure lhs, IFailure rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Failure lhs, IFailure rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Remainder);
        }

        public override string ToString()
        {
            return string.Format("Failure({0}, {1})", Remainder, Type);
        }

        public bool Equals(IFailure other)
        {
            if (object.ReferenceEquals(other, null))
                return false;
            return Remainder.Equals(other.Remainder);
        }
    }

    public enum FailureType
    {
        UnexpectedEndOfStream,
        UnexpectedChars,
        UnexpectedTrailingChars,
        SyntaxError,
        ExpectedLiteral
    }

    public static class Result
    {
        public static ISuccess Success(Input remainder, object value)
        {
            return new Success {
                Remainder = remainder,
                Value = value,
            };
        }

        public static IFailure Failure(Input remainder, FailureType type)
        {
            return new Failure {
                Remainder = remainder,
                Type = type,
            };
        }
    }
}
