namespace QuizData.ExcelReport
{
    public class ExcelRange
    {
        public bool IsStartPositionSet { get; private set; }
        public int StartPositionLine { get; private set; }
        public int StartPositionColumn { get; private set; }

        public bool IsEndPositionSet { get; private set; }
        public int EndPositionLine { get; private set; }
        public int EndPositionColumn { get; private set; }

        public ExcelWorksheetWrapper WorkSheet { get; }

        public ExcelRange(ExcelWorksheetWrapper worksheet)
        {
            WorkSheet = worksheet;
        }

        public void SetStartPosition(int column, int line)
        {
            IsStartPositionSet = true;
            StartPositionLine = line;
            StartPositionColumn = column;
        }

        public void SetEndPosition(int column, int line)
        {
            IsEndPositionSet = true;
            EndPositionLine = line;
            EndPositionColumn = column;
        }

        public bool IsReady()
        {
            return IsStartPositionSet && IsEndPositionSet;
        }

        public override string ToString()
        {
            return IsReady() ?
                string.Format("'{4}'!{0}{1}:{2}{3}",
                (char)(StartPositionColumn + 64), StartPositionLine,
                (char)(EndPositionColumn + 64), EndPositionLine,
                WorkSheet.Worksheet.Name)
                : null;
        }

        public static implicit operator string(ExcelRange range)
        {
            return range.ToString();
        }
    }
}
