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
        /// <returns>Linear equation coefficients and the error</returns>
        public static (double k, double b, double r) GetLinearApproximation(IEnumerable<double> xSeq, IEnumerable<double> ySeq)
        {
            var n = xSeq.Count();

            if (n != ySeq.Count())
                throw new ArgumentException("x[] and y[] must be the same size");

            if (n < 2)
                throw new ArgumentException("Arrays must have at least 2 elements");

            var sumX = xSeq.Sum();
            var sumY = ySeq.Sum();
            var sumX2 = xSeq.Aggregate((total, next) => total += Math.Pow(next, 2));
            
            var sumXY = 0.0;
            for (var i = 0; i < n; i++)
            {
                sumXY += xSeq.ElementAt(i) * ySeq.ElementAt(i);
            }

            var k = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
            var b = (sumY - k * sumX) / n;

            return (k, b, CalculateError(xSeq, ySeq, x => k * x + b));
        }

        public static double CalculateError(IEnumerable<double> xSeq, IEnumerable<double> ySeq, Func<double, double> fAppr)
        {
            var sumNsquares = xSeq.Zip(ySeq, (x, y) => (x, y)).Aggregate<(double x, double y), (int n, double s)>((0, 0), (a, p) =>
                {
                    var r = (p.y - fAppr(p.x));
                    return (a.n + 1, a.s + r * r);
                });
            return Math.Sqrt(sumNsquares.s / sumNsquares.n);
        }
    }
}
