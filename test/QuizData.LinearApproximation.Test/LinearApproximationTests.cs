using Xunit;

namespace QuizData.LinearApproximation.Test
{
    public class LinearApproximationTests
    {
        [Fact]
        public void TestLinearFunction()
        {
            // Take points of line y = 11x + 29
            var x = new double[] { 0, 1, 2, 3, 4 };
            var y = new double[] { 29, 40, 51, 62, 73 };

            (var k, var b) = LinearApproximation.GetLinearApproximation(x, y);

            Assert.Equal(11, k);
            Assert.Equal(29, b);
        }

        [Fact]
        public void TestNonMonotonicFunction()
        {
            var x = new double[] { 0, 1, 2, 3, 4, 5 };
            var y = new double[] { 5, 6, 5, 6, 5, 6 };

            (var k, var b) = LinearApproximation.GetLinearApproximation(x, y);

            Assert.Equal(0.0857, (int)(k * 10_000) / 10_000.0);
            Assert.Equal(5.2857, (int)(b * 10_000) / 10_000.0);
        }

        [Fact]
        public void TestRandomData()
        {
            var x = new double[] { 0, 1, 2, 3, 4 };
            var y = new double[] { 79, 5, 37, 20, 21 };

            (var k, var b) = LinearApproximation.GetLinearApproximation(x, y);

            Assert.Equal(-10.1, k);
            Assert.Equal(52.6, b);
        }
    }
}
