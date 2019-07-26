namespace QuizData.Analyser.Models
{
    /// <summary>
    /// Stores the number of numerics belonging the interval from the left border including
    /// to the right border excluding
    /// </summary>
    public class NumericDistributionInterval
    {
        public double LeftBorder { get; }
        public double RightBorder { get; }
        public uint NumericsAmount { get; private set; }

        public NumericDistributionInterval(double leftBorder, double rightBorder)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            NumericsAmount = 0;
        }

        public bool DoesNumericBelongToRange(double numeric)
        {
            return numeric >= LeftBorder && numeric < RightBorder;
        }

        public bool AddNumeric(double numeric)
        {
            if (DoesNumericBelongToRange(numeric))
            {
                NumericsAmount++;
                return true;
            }
            return false;
        }
    }
}
