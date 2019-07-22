namespace QuizData.Analyser.Models
{
    public class DoubleNumericDistributionPart
    {
        public NumericDistributionPart Part1 { get; set; }
        public NumericDistributionPart Part2 { get; set; }
        public uint NumericsAmount { get; private set; }

        public DoubleNumericDistributionPart(NumericDistributionPart part1,
            NumericDistributionPart part2)
        {
            Part1 = part1;
            Part2 = part2;
            NumericsAmount = 0;
        }

        public bool DoesNumericsBelongToRange(double numeric1, double numeric2)
        {
            return numeric1 >= Part1.LeftBorder && numeric1 < Part1.RightBorder
                && numeric2 >= Part2.LeftBorder && numeric2 < Part2.RightBorder;
        }

        public bool AddNumeric(double numeric1, double numeric2)
        {
            if (DoesNumericsBelongToRange(numeric1, numeric2))
            {
                NumericsAmount++;
                return true;
            }
            return false;
        }
    }
}
