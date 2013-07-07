using System.Collections.Generic;
using Xunit;

namespace Parler.Tests
{
    public class MiscTests
    {
        [Fact]
        public void TestMiscIsComplement()
        {
            ISet<string> cset = new ComplementarySet<string>();
            Assert.True(cset.IsComplement());
        }

        [Fact]
        public void TestMiscTrace()
        {
            string traceOutput = null;
            Misc.OnTrace = (s, args) => traceOutput = string.Format(s, args);
            Misc.Trace("Hello {0}!", "World");
            Misc.OnTrace = null;
            Misc.Trace("Hello {0}!", "World");
            Assert.Equal("Hello World!", traceOutput);
        }

        [Fact]
        public void TestMiscDictionaryGetOrAdd()
        {
            string key0 = "Blinky";

            var dict = new Dictionary<string, int>();

            int value0 = 0;
            dict.GetOrAdd(key0, v => value0 = v, () => 42);
            Assert.Equal(0, value0);
            dict.GetOrAdd(key0, v => value0 = v, () => 42);
            Assert.Equal(42, value0);
        }

        [Fact]
        public void TestMiscDictionaryGet()
        {
            string key0 = "Blinky";

            var dict = new Dictionary<string, int>() { { key0, 42 } };

            int value0 = 0;
            dict.Get(key0, v => { value0 = v; return true; }, () => value0 = 0);
            Assert.Equal(42, value0);
            dict.Get(key0, v => { value0 = v; return false; }, () => value0 = 0);
            Assert.Equal(0, value0);
        }

        [Fact]
        public void TestMiscGetHashCode()
        {
            var hash0 = Misc.GetHashCode("Hello", "World");
            var hash1 = Misc.GetHashCode("World", "Hello");
            var hash2 = Misc.GetHashCode("Hello", "World", "!");

            Assert.False(hash0 == hash1);
            Assert.False(hash0 == hash2);
        }
    }
}
