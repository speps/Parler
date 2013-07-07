using Parler.Parsers;
using System;
using System.Linq;
using Xunit;

namespace Parler.Tests.Parsers
{
    public class LiteralParserTests
    {
        [Fact]
        public void TestLiteralParserComputeFirst()
        {
            var p = new LiteralParser("Hello");
            var first = p.First();
            Assert.Equal(1, first.Count());
            Assert.Equal('H', first.First());
        }

        [Fact]
        public void TestLiteralParserParseSingleWordSuccess()
        {
            var p = new LiteralParser("Hello");
            var r = p.Parse(new Input("Hello World!"));
            Assert.True(r is Success);
            Assert.Equal("Hello", (r as Success).Value);
        }

        [Fact]
        public void TestLiteralParserParseSingleWordFailure()
        {
            var p = new LiteralParser("Hello");
            var r = p.Parse(new Input("World!"));
            Assert.True(r is Failure);
            Assert.Equal(FailureType.ExpectedLiteral, (r as Failure).Type);
        }

        [Fact]
        public void TestLiteralParserParseSingleWordFailureEOF()
        {
            var p = new LiteralParser("Hello");
            var r = p.Parse(new Input("Wo"));
            Assert.True(r is Failure);
            Assert.Equal(FailureType.UnexpectedEndOfStream, (r as Failure).Type);
        }

        [Fact]
        public void TestLiteralParserApplySingleWordSuccess()
        {
            var p = new LiteralParser("Hello");
            var r = p.Apply(new Input("Hello"));
            Assert.Equal(1, r.Count());
            Assert.True(r.First() is Success);
            Assert.Equal("Hello", (r.First() as Success).Value);
        }

        [Fact]
        public void TestLiteralParserApplySingleWordFailure()
        {
            var p = new LiteralParser("Hello");
            var r = p.Apply(new Input("World!"));
            Assert.Equal(1, r.Count());
            Assert.True(r.First() is Failure);
        }

        [Fact]
        public void TestLiteralParserApplySingleWordFailureTrailingChars()
        {
            var p = new LiteralParser("Hello");
            var r = p.Apply(new Input("Hello World!"));
            Assert.Equal(1, r.Count());
            Assert.True(r.First() is Failure);
            Assert.Equal(FailureType.UnexpectedTrailingChars, (r.First() as Failure).Type);
        }
    }
}
