using System;
using System.Collections.Generic;

namespace Parler
{
    public class ComplementarySet<T> : ISet<T>
    {
        private ISet<T> Without;

        public ComplementarySet()
        {
            Without = new HashSet<T>();
        }

        public ComplementarySet(IEnumerable<T> without)
        {
            Without = new HashSet<T>(without);
        }

        public ComplementarySet(ComplementarySet<T> other)
        {
            Without = new HashSet<T>(other.Without);
        }

        public bool Add(T item)
        {
            return Without.Remove(item);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other is ComplementarySet<T>)
            {
                Without.UnionWith((other as ComplementarySet<T>).Without);
            }
            else
            {
                Without.ExceptWith(other);
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            Without.Clear();
        }

        public bool Contains(T item)
        {
            return !Without.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new InvalidOperationException("Can't convert complementary set to array");
        }

        public int Count
        {
            get { return int.MaxValue; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return Without.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new InvalidOperationException("Can't iterate over complementary set");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new InvalidOperationException("Can't iterate over complementary set");
        }
    }
}
