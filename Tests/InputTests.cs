using Xunit;

namespace Parler.Tests
{
    public class InputTests
    {
        public const string SampleInput = "Hello World!";

        [Fact]
        public void TestInputAdvance()
        {
            var inputStart = new Input(SampleInput);
            var input = inputStart.Advance();
            Assert.Equal('H', inputStart.Current);
            Assert.Equal('e', input.Current);
            input = input.Advance();
            Assert.Equal('l', input.Current);
            input = input.Advance();
            Assert.Equal('l', input.Current);
        }

        [Fact]
        public void TestInputEquality()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            Assert.True(input0.Equals(input1), "object.Equals");
            Assert.True(input0 == input1, "operator==");
            Assert.False(input0 != input1, "operator!=");

            input0 = input0.Advance();

            Assert.False(input0.Equals(input1), "advanced so not object.Equals");
            Assert.False(input0 == input1, "advanced so not operator==");
            Assert.True(input0 != input1, "advanced so operator!=");

            Assert.False(input0 == null, "== null returns false");
            Assert.True(input0 != null, "!= null returns true");
            Assert.False(input0.Equals(null), "Equals(null) returns false");
            Assert.False(input0.Equals("42"), "Equals(string) returns false");

            input0 = null;
            Assert.True(input0 == null, "== null returns true if null");
            Assert.False(input0 != null, "!= null returns false if null");
        }

        [Fact]
        public void TestInputHashCode()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            Assert.True(input0.GetHashCode() == input1.GetHashCode(), "same GetHashCode");

            input0 = input0.Advance();

            Assert.False(input0.GetHashCode() == input1.GetHashCode(), "different GetHashCode if advanced");
        }

        [Fact]
        public void TestInputTake()
        {
            var input = new Input(SampleInput);
            input = input.Advance();

            int positionBeforeTake = input.Position;
            var take5 = input.Take(5);

            Assert.True(positionBeforeTake == input.Position, "Take doesn't change Position");
            Assert.Equal("ello ", take5);

            var takeMore = input.Take(input.Source.Length + 4);
            Assert.Equal("ello World!", takeMore);
        }

        [Fact]
        public void TestInputDrop()
        {
            var input = new Input(SampleInput);
            input = input.Advance();

            var dropped = input.Drop(5);

            Assert.Equal('W', dropped.Current);
            Assert.Equal(6, dropped.Position);

            dropped = input.Drop(input.Source.Length + 4);

            Assert.True(dropped.EOF, "dropping past end sets EOF");
        }
    }
}
