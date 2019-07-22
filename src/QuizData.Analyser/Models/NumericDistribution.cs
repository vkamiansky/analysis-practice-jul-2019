using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser.Models
{
    /// <summary>
    /// Represents number distribution
    /// </summary>
    public class NumericDistribution
    {
        public List<NumericDistributionPart> Parts { get; set; }

        /// <summary>
        /// Creates distibution with no data
        /// </summary>
        /// <param name="leftBorder">Left border on the number axis</param>
        /// <param name="rightBorder">Right border on the number axis</param>
        /// <param name="rangesCount">The number of ranges into which the numbers will be divided</param>
        public NumericDistribution(double leftBorder, double rightBorder, uint rangesCount)
        {
            Parts = new List<NumericDistributionPart>((int)rangesCount);

            double step = (rightBorder - leftBorder) / rangesCount;
            var i = 0;
            for ( ; i < rangesCount - 1; i++)
            {
                Parts.Add(new NumericDistributionPart(leftBorder + step * i,
                    leftBorder + step * (i + 1)));
            }
            Parts.Add(new NumericDistributionPart(leftBorder + step * i,
                    rightBorder + 0.0000000001));
        }

        /// <summary>
        /// Adds new numeric to the distribution data
        /// </summary>
        /// <param name="numeric">New numeric</param>
        /// <returns>Returns True if the number has been added</returns>
        public bool AddNumeric(double numeric)
        {
            foreach (var part in Parts)
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
                return Parts.First().LeftBorder;
            }
        }

        public double RightBorder
        {
            get
            {
                return Parts.Last().RightBorder;
            }
        }
    }
}
