using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizData.LinearApproximation
{
    public class LinearApproximation
    {
        /// <summary>
        /// Builds linear approximation and returns linear equation coefficients
        /// </summary>
        /// <returns>Linear equation coefficients</returns>
        public static (double k, double b) GetLinearApproximation(IEnumerable<double> x, IEnumerable<double> y)
        {
            var n = x.Count();

            if (n != y.Count())
                throw new ArgumentException("x[] and y[] must be the same size");

            if (n < 2)
                throw new ArgumentException("Arrays must have at least 2 elements");

            var sumX = x.Sum();
            var sumY = y.Sum();
            var sumX2 = x.Aggregate((total, next) => total += Math.Pow(next, 2));
            
            var sumXY = 0.0;
            for (var i = 0; i < n; i++)
            {
                sumXY += x.ElementAt(i) * y.ElementAt(i);
            }

            var k = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
            var b = (sumY - k * sumX) / n;

            return (k, b);
        }
    }
}
