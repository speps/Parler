using System;
using Parler;
using Xunit;
using System.Collections.Generic;

namespace Parler.Tests
{
    public class ComplementarySetTests
    {
        public const string Value0 = "Blinky";
        public const string Value1 = "Pinky";
        public const string Value2 = "Inky";
        public const string Value3 = "Clyde";

        [Fact]
        public void TestComplementarySetContains()
        {
            var set = new ComplementarySet<string>();
            Assert.True(set.Contains(Value0), "set contains all values at construction");
        }

        [Fact]
        public void TestComplementarySetAdd()
        {
            var set = new ComplementarySet<string>();
            set.Add(Value1);
            Assert.True(set.Contains(Value1), "set contains value added");
        }

        [Fact]
        public void TestComplementarySetRemove()
        {
            var set = new ComplementarySet<string>();
            set.Remove(Value2);
            Assert.False(set.Contains(Value2), "set doesn't contain value removed");
        }

        [Fact]
        public void TestComplementarySetWithout()
        {
            var set = new ComplementarySet<string>(new string[] { Value0, Value1 });
            Assert.False(set.Contains(Value0), "constructed set doesn't contain value");
            Assert.False(set.Contains(Value1), "constructed set doesn't contain value");
        }

        [Fact]
        public void TestComplementarySetUnionWith()
        {
            var cset = new ComplementarySet<string>(new string[] { Value0, Value1 });

            Assert.False(cset.Contains(Value0), "set doesn't contain Value0");
            Assert.False(cset.Contains(Value1), "set doesn't contain Value1");

            var set = new HashSet<string>(new string[] { Value1, Value2 });

            cset.UnionWith(set);

            Assert.False(cset.Contains(Value0), "union still doesn't contain Value0");
            Assert.True(cset.Contains(Value1), "union contains Value1");
            Assert.True(cset.Contains(Value2), "union contains Value2");
        }

        [Fact]
        public void TestComplementarySetCount()
        {
            var set = new ComplementarySet<string>();
            Assert.Equal(int.MaxValue, set.Count);
        }

        [Fact]
        public void TestComplementarySetNotImplemented()
        {
            var cset = new ComplementarySet<string>();
            var ienum = new string[] { "Hello", "World!" };
            Assert.Throws<NotImplementedException>(() => cset.ExceptWith(ienum));
            Assert.Throws<NotImplementedException>(() => cset.IntersectWith(ienum));
            Assert.Throws<NotImplementedException>(() => cset.IsProperSubsetOf(ienum));
            Assert.Throws<NotImplementedException>(() => cset.IsProperSupersetOf(ienum));
            Assert.Throws<NotImplementedException>(() => cset.IsSubsetOf(ienum));
            Assert.Throws<NotImplementedException>(() => cset.IsSupersetOf(ienum));
            Assert.Throws<NotImplementedException>(() => cset.Overlaps(ienum));
            Assert.Throws<NotImplementedException>(() => cset.SetEquals(ienum));
            Assert.Throws<NotImplementedException>(() => cset.SymmetricExceptWith(ienum));
        }

        [Fact]
        public void TestComplementarySetInvalidOperation()
        {
            var cset = new ComplementarySet<string>();
            var ienum = new string[] { "Hello", "World!" };

            Assert.Throws<InvalidOperationException>(() => cset.CopyTo(ienum, 0));
            Assert.Throws<InvalidOperationException>(() => { foreach (var s in cset) s.ToUpper(); });
        }
    }
}
