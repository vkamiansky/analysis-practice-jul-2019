﻿using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
    public class DoubleNumericDistribution
    {
        public List<DoubleNumericDistributionPart> Parts { get; }

        public DoubleNumericDistribution(double leftBorder1, double rightBorder1,
            double leftBorder2, double rightBorder2, uint rangesCount)
        {
            Parts = new List<DoubleNumericDistributionPart>((int)rangesCount);

            double step1 = (rightBorder1 - leftBorder1) / rangesCount;
            double step2 = (rightBorder2 - leftBorder2) / rangesCount;

            var i = 0;
            for (; i < rangesCount - 1; i++)
            {
                var j = 0;
                for (; j < rangesCount - 1; j++)
                {
                    var part1 = new NumericDistributionPart(leftBorder1 + step1 * i, leftBorder1 + step1 * (i + 1));
                    var part2 = new NumericDistributionPart(leftBorder2 + step2 * j, leftBorder2 + step2 * (j + 1));
                    Parts.Add(new DoubleNumericDistributionPart(part1, part2));
                }
                var lastPart1 = new NumericDistributionPart(leftBorder1 + step1 * i, leftBorder1 + step1 * (i + 1));
                var lastPart2 = new NumericDistributionPart(leftBorder2 + step2 * j, rightBorder2 + 0.0000000001);
                Parts.Add(new DoubleNumericDistributionPart(lastPart1, lastPart2));
            }
            var k = 0;
            for (; k < rangesCount - 1; k++)
            {
                var part1 = new NumericDistributionPart(leftBorder1 + step1 * i, rightBorder1 + 0.0000000001);
                var part2 = new NumericDistributionPart(leftBorder2 + step2 * k, leftBorder2 + step2 * (k + 1));
                Parts.Add(new DoubleNumericDistributionPart(part1, part2));
            }
            var a = new NumericDistributionPart(leftBorder1 + step1 * i, rightBorder1 + 0.0000000001);
            var b = new NumericDistributionPart(leftBorder2 + step2 * k, rightBorder2 + 0.0000000001);
            Parts.Add(new DoubleNumericDistributionPart(a, b));
        }

        public bool AddNumerics(double numeric1, double numeric2)
        {
            foreach (var part in Parts)
            {
                if (part.AddNumeric(numeric1, numeric2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
