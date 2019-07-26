using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser.Models
{
    /// <summary>
    /// Represents numeric distribution
    /// </summary>
    public class NumericDistribution
    {
        public List<NumericDistributionInterval> Intervals { get; set; }

        /// <summary>
        /// Creates distibution with no data
        /// </summary>
        /// <param name="leftBorder">Left border on the number axis</param>
        /// <param name="rightBorder">Right border on the number axis</param>
        /// <param name="intervalsAmount">The number of intervals into which the numbers will be divided</param>
        public NumericDistribution(double leftBorder, double rightBorder, uint intervalsAmount)
        {
            Intervals = new List<NumericDistributionInterval>((int)intervalsAmount);

            double step = (rightBorder - leftBorder) / intervalsAmount;
            var i = 0;
            for ( ; i < intervalsAmount - 1; i++)
            {
                Intervals.Add(new NumericDistributionInterval(leftBorder + step * i,
                    leftBorder + step * (i + 1)));
            }
            Intervals.Add(new NumericDistributionInterval(leftBorder + step * i,
                    rightBorder + 0.0000000001));
        }

        /// <summary>
        /// Adds new numeric to the distribution data
        /// </summary>
        /// <param name="numeric">New numeric</param>
        /// <returns>Returns True if the number has been added</returns>
        public bool AddNumeric(double numeric)
        {
            foreach (var part in Intervals)
            {
                if (part.AddNumeric(numeric))
                {
                    return true;
                }
            }

            return false;
        }

        public double LeftBorder
        {
            get
            {
                return Intervals.First().LeftBorder;
            }
        }

        public double RightBorder
        {
            get
            {
                return Intervals.Last().RightBorder;
            }
        }
    }
}
