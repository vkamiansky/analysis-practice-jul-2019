namespace QuizData.Analyser.Models
{
    public class DoubleNumericDistributionPart
    {
        public NumericDistributionPart Part1 { get; set; }
        public NumericDistributionPart Part2 { get; set; }
        public uint NumericsAmount { get; private set; }
        public double? SigmaMin { get; private set; }
        public double? SigmaMax { get; private set; }

        public DoubleNumericDistributionPart(NumericDistributionPart part1,
            NumericDistributionPart part2)
        {
            Part1 = part1;
            Part2 = part2;
            NumericsAmount = 0;
            SigmaMin = null;
            SigmaMax = null;
        }

        public bool DoesNumericsBelongToRange(double numeric1, double numeric2)
        {
            return numeric1 >= Part1.LeftBorder && numeric1 < Part1.RightBorder
                && numeric2 >= Part2.LeftBorder && numeric2 < Part2.RightBorder;
        }

        public bool AddNumeric(double numeric1, double numeric2, double sigma)
        {
            if (DoesNumericsBelongToRange(numeric1, numeric2))
            {
                NumericsAmount++;
                SigmaMin = System.Math.Min(SigmaMin ?? double.MaxValue, sigma);
                SigmaMax = System.Math.Max(SigmaMax ?? 0, sigma);
                return true;
            }
            return false;
        }
    }
}
