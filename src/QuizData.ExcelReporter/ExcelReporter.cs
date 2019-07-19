using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using QuizData.Analyser.Models;
using System.Collections.Generic;

namespace QuizData.ExcelReport
{
	public class ExcelReporter
	{
        private ExcelPackage _package;
        private ExcelWorksheet _main;
        private ExcelWorksheet _temp;
        private ExcelWorksheet _questions;
        private int _currentLine;
        private int _tempCurrentLine;
        private int _questionsCurrentLine;

        public ExcelReporter()
        {
            _package = new ExcelPackage();
            _temp = _package.Workbook.Worksheets.Add("_Temp");
            _main = _package.Workbook.Worksheets.Add("Главная");
            _questions = _package.Workbook.Worksheets.Add("Вопросы");
            _currentLine = 1;
            _tempCurrentLine = 1;
            _questionsCurrentLine = 1;
        }

        public void BuildAttemptDistributionChart(DataAnalyserReport report)
        {
            var startLineTemp = _tempCurrentLine;

            for (var i = 0; i < report.AttemptDistribution.Length; i++)
            {
                if (report.AttemptDistribution[i] != 0)
                {
                    _temp.Cells[_tempCurrentLine, 1].Value = i + 1;
                    _temp.Cells[_tempCurrentLine, 2].Value = report.AttemptDistribution[i];
                    _tempCurrentLine++;
                }
            }

            var chart = _main.Drawings.AddChart("AttemptDistribution",
                OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
            chart.Title.Text = "Распределение попыток";
            var address1 = string.Format("B{0}:B{1}", startLineTemp, _tempCurrentLine - 1);
            var address2 = string.Format("A{0}:A{1}", startLineTemp, _tempCurrentLine - 1);
            chart.Series.Add(ExcelRange.GetFullAddress("_Temp", address1),
                ExcelRange.GetFullAddress("_Temp", address2));
            chart.Legend.Remove();

            chart.From.Row = _currentLine;
            chart.To.Row = _currentLine + 10;
            _currentLine += 11;
        }

        public void BuildResultDistributionChart(DataAnalyserReport report)
        {
            var startLineTemp = _tempCurrentLine;

            var list = report.ResultDistribution.ToList();
            list.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            foreach (var el in list)
            {
                _temp.Cells[_tempCurrentLine, 1].Value = el.Key;
                _temp.Cells[_tempCurrentLine, 2].Value = el.Value;
                _tempCurrentLine++;
            }

            var chart = _main.Drawings.AddChart("ResultDistribution",
                OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
            chart.Title.Text = "Распределение результатов";
            var address1 = string.Format("B{0}:B{1}", startLineTemp, _tempCurrentLine - 1);
            var address2 = string.Format("A{0}:A{1}", startLineTemp, _tempCurrentLine - 1);
            chart.Series.Add(ExcelRange.GetFullAddress("_Temp", address1),
                ExcelRange.GetFullAddress("_Temp", address2));
            chart.Legend.Remove();

            chart.From.Row = _currentLine;
            chart.To.Row = _currentLine + 10;
            _currentLine += 11;
        }

        public void Question(KeyValuePair<string, QuestionStatistics> qStatistics)
        {
            _questions.Cells[_questionsCurrentLine, 1].Value = qStatistics.Key;
            _questionsCurrentLine++;

            _questions.Cells[_questionsCurrentLine, 1].Value = "Правильных ответов:";
            _questions.Cells[_questionsCurrentLine, 2].Value = qStatistics.Value.RightAnswersAmount;
            _questionsCurrentLine++;

            _questions.Cells[_questionsCurrentLine, 1].Value = "Неправильных ответов:";
            _questions.Cells[_questionsCurrentLine, 2].Value = qStatistics.Value.WrongAnswersAmount;
            _questionsCurrentLine++;

            _questions.Cells[_questionsCurrentLine, 1].Value = "Всего ответов:";
            _questions.Cells[_questionsCurrentLine, 2].Value = qStatistics.Value.RightAnswersAmount +
                qStatistics.Value.WrongAnswersAmount;
            _questionsCurrentLine++;

            _questions.Cells[_questionsCurrentLine, 1].Value = "Правильный ответ:";
            _questions.Cells[_questionsCurrentLine, 2].Value = qStatistics.Value.RightAnswerIndex + 1;
            _questionsCurrentLine++;

            var startLineTemp = _tempCurrentLine;

            for (var i = 0; i < qStatistics.Value.AnswersDistribution.Length; i++)
            {
                _temp.Cells[_tempCurrentLine, 1].Value = i + 1;
                _temp.Cells[_tempCurrentLine, 2].Value = qStatistics.Value.AnswersDistribution[i];
                _tempCurrentLine++;
            }

            var chart = _questions.Drawings.AddChart("QuestionStatistics" + qStatistics.Key.GetHashCode(),
                OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
            chart.Title.Text = "Ответы пользователей";
            var address1 = string.Format("B{0}:B{1}", startLineTemp, _tempCurrentLine - 1);
            var address2 = string.Format("A{0}:A{1}", startLineTemp, _tempCurrentLine - 1);
            chart.Series.Add(ExcelRange.GetFullAddress("_Temp", address1),
                ExcelRange.GetFullAddress("_Temp", address2));
            chart.Legend.Remove();

            chart.From.Row = _questionsCurrentLine;
            chart.To.Row = _questionsCurrentLine + 10;
            _questionsCurrentLine += 12;
        }

        public void ToFile(string path, DataAnalyserReport report)
		{
            _main.Cells[_currentLine, 1].Value = "Всего тестов: ";
			_main.Cells[_currentLine, 2].Value = report.TotalAmountOfTests;
            _currentLine++;

            _main.Cells[_currentLine, 1].Value = "Количество уникальных e-mail'ов: ";
            _main.Cells[_currentLine, 2].Value = report.AmountOfUniqueEmails;
            _currentLine++;

            BuildAttemptDistributionChart(report);
            BuildResultDistributionChart(report);

            foreach (var el in report.QuestionStatistics)
                Question(el);

			//var max = 0U;
			//var userWithMax = "";
			//foreach (var pStatistics in report.PersonStatistics)
			//{
			//	if (pStatistics.Value.AmountOfAttempts > max)
			//	{
			//		max = pStatistics.Value.AmountOfAttempts;
			//		userWithMax = pStatistics.Key;
			//	}
			//}

			

            

   //         _main.Cells[20, 1].Value = string.Format("Результаты пользователя {0}", userWithMax);
			//currentLineNumber = 21;
			//for (var i = 0; i < report.PersonStatistics[userWithMax].Results.Count; i++)
			//{
			//	_main.Cells[currentLineNumber, 1].Value = i + 1;
			//	_main.Cells[currentLineNumber, 2].Value = report.PersonStatistics[userWithMax].Results[i];
			//	currentLineNumber++;
			//}

			//var chart2 = _main.Drawings.AddChart("chart2", OfficeOpenXml.Drawing.Chart.eChartType.Line);
			//var chartSerie = chart2.Series.Add(ExcelRange.GetAddress(21, 2, 62, 2), ExcelRange.GetAddress(21, 1, 62, 1));
			//var trend = chartSerie.TrendLines.Add(OfficeOpenXml.Drawing.Chart.eTrendLine.Linear);
			//trend.DisplayRSquaredValue = false;

			_package.SaveAs(new FileInfo(path));
		}
	}
}
