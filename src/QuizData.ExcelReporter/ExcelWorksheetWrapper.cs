using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace QuizData.ExcelReport
{
    public class ExcelWorksheetWrapper
    {
        public ExcelWorksheet Worksheet { get; }

        private int _currentLine;
        private int _currentColumn;

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
            _currentLine = 1;
            _currentColumn = 1;
        }

        private void CopyToPrevious()
        {
            _previousStepLine = _currentLine;
            _previousStepColumn = _currentColumn;
            CanGoBack = true;
        }

        public void Write(object value)
        {
            Worksheet.Cells[_currentLine, _currentColumn].Value = value;
            CopyToPrevious();
            _currentColumn++;
        }

        public void WriteLine()
        {
            CopyToPrevious();
            _currentLine++;
            _currentColumn = 1;
        }

        public void WriteLine(object value)
        {
            Worksheet.Cells[_currentLine, _currentColumn].Value = value;
            CopyToPrevious();
            _currentLine++;
            _currentColumn = 1;
        }

        public void GoBack()
        {
            if (!CanGoBack)
                throw new System.Exception("Something went wrong...");

            _currentColumn = _previousStepColumn;
            _currentLine = _previousStepLine;

            CanGoBack = false;
        }

        public void SetPos1()
        {
            _pos1Line = _currentLine;
            _pos1Column = _currentColumn;
        }

        public void SetPos2()
        {
            _pos2Line = _currentLine;
            _pos2Column = _currentColumn;
        }

        public void CreateChart(string chartName, string chartTitle)
        {
            CreateChart(chartName, chartTitle, this);
        }

        public void CreateChart(string chartName, string chartTitle, ExcelWorksheetWrapper to)
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

            to.PlaceChart(chart);
        }

        public void PlaceChart(ExcelChart chart)
        {
            chart.From.Row = _currentLine;
            chart.To.Row = _currentLine + 10;
            _currentLine += 11;
        }
    }
}
