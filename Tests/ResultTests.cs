using Xunit;

namespace Parler.Tests
{
    public class ResultTests
    {
        public const string SampleInput = "Hello World!";

        [Fact]
        public void TestSuccessEqualityOtherValues()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, ISuccess won't work
            var success0 = Result.Success(input0, null) as Success;
            var success1 = Result.Success(input1, "ell") as Success;

            Assert.False(success0.Equals(null), "object.Equals(null)");
            Assert.False(success0.Equals("42"), "object.Equals(string)");

            Assert.False(success0.Equals(success1), "lhs null value");
            Assert.False(success1.Equals(success0), "rhs null value");

            success0 = null;
            success1 = null;

            Assert.True(null == success1, "null == Success");
            Assert.True(success0 == null, "Success == null");
            Assert.True(success0 == success1, "null == null");
        }

        [Fact]
        public void TestSuccessEqualitySameInputSameValue()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, ISuccess won't work
            var success0 = Result.Success(input0, "Hell") as Success;
            var success1 = Result.Success(input1, "Hell") as Success;

            Assert.True(success0.Equals(success1), "ISuccess.Equals");
            Assert.True(success0 == success1, "operator==");
            Assert.False(success0 != success1, "operator!=");
            Assert.True(success0.GetHashCode() == success1.GetHashCode(), "GetHashCode");
        }

        [Fact]
        public void TestSuccessEqualitySameInputDifferentValue()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, ISuccess won't work
            var success0 = Result.Success(input0, "Hell") as Success;
            var success1 = Result.Success(input1, "ell") as Success;

            Assert.False(success0.Equals(success1), "ISuccess.Equals");
            Assert.False(success0 == success1, "operator==");
            Assert.True(success0 != success1, "operator!=");
            Assert.False(success0.GetHashCode() == success1.GetHashCode(), "GetHashCode");
        }

        [Fact]
        public void TestSuccessEqualityDifferentInputSameValue()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, ISuccess won't work
            var success0 = Result.Success(input0.Advance(), "Hell") as Success;
            var success1 = Result.Success(input1, "Hell") as Success;

            Assert.False(success0.Equals(success1), "ISuccess.Equals");
            Assert.False(success0 == success1, "operator==");
            Assert.True(success0 != success1, "operator!=");
            Assert.False(success0.GetHashCode() == success1.GetHashCode(), "GetHashCode");
        }

        [Fact]
        public void TestSuccessEqualityDifferentInputDifferentValue()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, ISuccess won't work
            var success0 = Result.Success(input0.Advance(), "Hell") as Success;
            var success1 = Result.Success(input1, "ell") as Success;

            Assert.False(success0.Equals(success1), "ISuccess.Equals");
            Assert.False(success0 == success1, "operator==");
            Assert.True(success0 != success1, "operator!=");
            Assert.False(success0.GetHashCode() == success1.GetHashCode(), "GetHashCode");
        }

        [Fact]
        public void TestFailureEqualityOtherValues()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, IFailure won't work
            var failure0 = Result.Failure(input0, FailureType.SyntaxError) as Failure;
            var failure1 = Result.Failure(input1, FailureType.ExpectedLiteral) as Failure;

            Assert.False(failure0.Equals(null), "object.Equals(null)");
            Assert.False(failure0.Equals("42"), "object.Equals(string)");

            failure0 = null;
            failure1 = null;

            Assert.True(null == failure1, "null == Failure");
            Assert.True(failure0 == null, "Failure == null");
            Assert.True(failure0 == failure1, "null == null");
        }

        [Fact]
        public void TestFailureEqualitySameInput()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, IFailure won't work
            var failure0 = Result.Failure(input0, FailureType.SyntaxError) as Failure;
            var failure1 = Result.Failure(input1, FailureType.ExpectedLiteral) as Failure;

            Assert.True(failure0.Equals(failure1), "IFailure.Equals");
            Assert.True(failure0 == failure1, "operator==");
            Assert.False(failure0 != failure1, "operator!=");
            Assert.True(failure0.GetHashCode() == failure1.GetHashCode(), "GetHashCode");
        }

        [Fact]
        public void TestFailureEqualityDifferentInput()
        {
            var input0 = new Input(SampleInput);
            var input1 = new Input(SampleInput);

            // explicit cast because == is compile time bound, IFailure won't work
            var failure0 = Result.Failure(input0.Advance(), FailureType.SyntaxError) as Failure;
            var failure1 = Result.Failure(input1, FailureType.ExpectedLiteral) as Failure;

            Assert.False(failure0.Equals(failure1), "IFailure.Equals");
            Assert.False(failure0 == failure1, "operator==");
            Assert.True(failure0 != failure1, "operator!=");
            Assert.False(failure0.GetHashCode() == failure1.GetHashCode(), "GetHashCode");
        }
    }
}
