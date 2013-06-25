using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parler
{
    public interface IResult
    {
        Input Remainder { get; }
    }

    public interface IResult<out T> : IResult
    {
        
    }

    public interface ISuccess : IResult
    {
        
    }

    public interface ISuccess<out T> : ISuccess, IResult<T>
    {
        T Value { get; }
    }

    public class Success<T> : ISuccess<T>
    {
        public Input Remainder { get; internal set; }
        public T Value { get; internal set; }

        public override bool Equals(object obj)
        {
            var other = obj as ISuccess;
            if (other == null)
                return false;
            var otherType = other.GetType();
            var otherValueProp = otherType.GetProperty("Value");
            if (otherValueProp == null)
                return false;
            if (!otherValueProp.PropertyType.Equals(typeof(T)))
                return false;
            var otherValue = otherValueProp.GetValue(other, null);
            return Remainder.Equals(other.Remainder) && Value.Equals(otherValue);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Remainder, Value);
        }
    }

    public interface IFailure : IResult
    {
    }

    public interface IFailure<out T> : IFailure, IResult<T>
    {
        
    }

    public class Failure<T> : IFailure<T>
    {
        public Input Remainder { get; internal set; }

        public override bool Equals(object obj)
        {
            var other = obj as IFailure;
            if (other == null)
                return false;
            return Remainder.Equals(other.Remainder);
        }

        public override int GetHashCode()
        {
            return Misc.GetHashCode(Remainder);
        }
    }

    public static class Result
    {
        public static T As<T>(this IResult result)
        {
            if (result is ISuccess<T>)
            {
                return (result as ISuccess<T>).Value;
            }
            throw new ArgumentException("result is not an ISuccess", "result");
        }

        public static ISuccess<T> Success<T>(Input remainder, T value)
        {
            return new Success<T> {
                Remainder = remainder,
                Value = value,
            };
        }

        public static IFailure<T> Failure<T>(Input remainder)
        {
            return new Failure<T> {
                Remainder = remainder,
            };
        }
    }
}
