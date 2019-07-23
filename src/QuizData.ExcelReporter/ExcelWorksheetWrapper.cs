using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace QuizData.ExcelReport
{
    public class ExcelWorksheetWrapper
    {
        public ExcelWorksheet Worksheet { get; }

        public int CurrentLine { get; private set; }
        public int CurrentColumn { get; private set; }

        private int _pos1Line;
        private int _pos1Column;

        private int _pos2Line;
        private int _pos2Column;

        public bool CanGoBack { get; private set; }

        private int _previousStepLine;
        private int _previousStepColumn;

        public ExcelWorksheetWrapper(ExcelWorksheet worksheet)
        {
            Worksheet = worksheet;
            CurrentLine = 1;
            CurrentColumn = 1;
        }

        private void CopyToPrevious()
        {
            _previousStepLine = CurrentLine;
            _previousStepColumn = CurrentColumn;
            CanGoBack = true;
        }

        public void Write()
        {
            CopyToPrevious();
            CurrentColumn++;
        }

        public void Write(object value)
        {
            Worksheet.Cells[CurrentLine, CurrentColumn].Value = value;
            Write();
        }

        public void WriteLine()
        {
            CopyToPrevious();
            CurrentLine++;
            CurrentColumn = 1;
        }

        public void WriteLine(object value)
        {
            Worksheet.Cells[CurrentLine, CurrentColumn].Value = value;
            CopyToPrevious();
            CurrentLine++;
            CurrentColumn = 1;
        }

        public void GoBack()
        {
            if (!CanGoBack)
                throw new System.Exception("Something went wrong...");

            CurrentColumn = _previousStepColumn;
            CurrentLine = _previousStepLine;

            CanGoBack = false;
        }

        public void SetPos1()
        {
            _pos1Line = CurrentLine;
            _pos1Column = CurrentColumn;
        }

        public void SetPos2()
        {
            _pos2Line = CurrentLine;
            _pos2Column = CurrentColumn;
        }

        public void CreateChart(string chartName, string chartTitle, int height = 10)
        {
            CreateChart(chartName, chartTitle, this, height);
        }

        public void CreateChart(string chartName, string chartTitle, ExcelWorksheetWrapper to, int height = 10)
        {
            var chart = to.Worksheet.Drawings.AddChart(chartName, eChartType.ColumnClustered);
            chart.Title.Text = chartTitle;
            var address1 = string.Format("{0}{1}:{2}{3}",
                (char)(_pos2Column + 64), _pos1Line, (char)(_pos2Column + 64), _pos2Line);
            var address2 = string.Format("{0}{1}:{2}{3}",
                (char)(_pos1Column + 64), _pos1Line, (char)(_pos1Column + 64), _pos2Line);
            chart.Series.Add(ExcelRange.GetFullAddress(Worksheet.Name, address1),
                ExcelRange.GetFullAddress(Worksheet.Name, address2));
            chart.Legend.Remove();

            to.PlaceChart(chart, height);
        }

        public void PlaceChart(ExcelChart chart, int height = 10)
        {
            chart.From.Row = CurrentLine;
            chart.To.Row = CurrentLine + height;
            CurrentLine += height + 1;
        }
    }
}
