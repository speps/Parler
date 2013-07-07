using System;
using System.Collections.Generic;

namespace Parler
{
    public static class Misc
    {
        public static Action<string, object[]> OnTrace;

        public static void Trace(string format, params object[] args)
        {
            if (OnTrace != null)
            {
                OnTrace(format, args);
            }
        }

        public static readonly ISet<char> UniversalCharSet = new ComplementarySet<char>();

        public static bool IsComplement<T>(this ISet<T> set)
        {
            return set is ComplementarySet<T>;
        }

        public static ISet<T> Union<T>(ISet<T> lhs, ISet<T> rhs)
        {
            if (lhs.IsComplement())
            {

            }
        }

        public static void GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> onFound, Func<TValue> onAdd)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                onFound(value);
            }
            else
            {
                value = onAdd();
                dictionary.Add(key, value);
            }
        }

        public static void Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue, bool> onFound, Action onElse)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                if (onFound(value))
                    return;
            }
            onElse();
        }

        public static int GetHashCode(params object[] args)
        {
            int hash = 17;
            foreach (var obj in args)
            {
                hash = hash * 31 + obj.GetHashCode();
            }
            return hash;
        }
    }
}
