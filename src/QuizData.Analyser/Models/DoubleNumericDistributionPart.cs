namespace QuizData.Analyser.Models
{
    public class DoubleNumericDistributionPart
    {
        public NumericDistributionInterval Interval1 { get; set; }
        public NumericDistributionInterval Interval2 { get; set; }
        public uint NumericsAmount { get; private set; }
        public double? SigmaMin { get; private set; }
        public double? SigmaMax { get; private set; }

        public DoubleNumericDistributionPart(NumericDistributionInterval interval1,
            NumericDistributionInterval interval2)
        {
            Interval1 = interval1;
            Interval2 = interval2;
            NumericsAmount = 0;
            SigmaMin = null;
            SigmaMax = null;
        }

        public bool DoesNumericsBelongToRange(double numeric1, double numeric2)
        {
            return numeric1 >= Interval1.LeftBorder && numeric1 < Interval1.RightBorder
                && numeric2 >= Interval2.LeftBorder && numeric2 < Interval2.RightBorder;
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
