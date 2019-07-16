using System;
using System.Linq;

namespace QuizData.LinearApproximation
{
    public class LinearApproximation
    {
        /// <summary>
        /// Builds linear approximation and returns linear equation coefficients
        /// </summary>
        /// <returns>Linear equation coefficients</returns>
        public static (double k, double b) GetLinearApproximation(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                throw new ArgumentException("x[] and y[] must be the same size");

            if (x.Length < 2)
                throw new ArgumentException("Arrays must have at least 2 elements");

            var n = x.Length;
            var sumX = x.Sum();
            var sumY = y.Sum();
            var sumX2 = x.Aggregate((total, next) => total += Math.Pow(next, 2));
            
            var sumXY = 0.0;
            for (var i = 0; i < n; i++)
            {
                sumXY += x[i] * y[i];
            }

            var k = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
            var b = (sumY - k * sumX) / n;

            return (k, b);
        }
    }
}
