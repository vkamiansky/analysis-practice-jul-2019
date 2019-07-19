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
        private ExcelWorksheetWrapper _main;
        private ExcelWorksheetWrapper _temp;
        private ExcelWorksheetWrapper _questions;

        public ExcelReporter()
        {
            _package = new ExcelPackage();
            _temp = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("_Temp"));
            _main = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("Главная"));
            _questions = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("Вопросы"));
        }

        public void BuildAttemptDistributionChart(DataAnalyserReport report)
        {
            _temp.SetPos1();

            for (var i = 0; i < report.AttemptDistribution.Length; i++)
            {
                if (report.AttemptDistribution[i] != 0)
                {
                    _temp.Write(i + 1);
                    _temp.WriteLine(report.AttemptDistribution[i]);
                }
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("AttemptDistribution", "Распределение попыток", _main);
        }

        public void BuildResultDistributionChart(DataAnalyserReport report)
        {
            _temp.SetPos1();

            var list = report.ResultDistribution.ToList();
            list.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            foreach (var el in list)
            {
                _temp.Write(el.Key);
                _temp.WriteLine(el.Value);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("ResultDistribution", "Распределение результатов", _main);
        }

        public void Question(KeyValuePair<string, QuestionStatistics> qStatistics)
        {
            _questions.WriteLine(qStatistics.Key);

            _questions.Write("Правильных ответов:");
            _questions.WriteLine(qStatistics.Value.RightAnswersAmount);

            _questions.Write("Неправильных ответов:");
            _questions.WriteLine(qStatistics.Value.WrongAnswersAmount);

            _questions.Write("Всего ответов:");
            _questions.WriteLine(qStatistics.Value.RightAnswersAmount +
                qStatistics.Value.WrongAnswersAmount);

            _questions.Write("Правильный ответ:");
            _questions.WriteLine(qStatistics.Value.RightAnswerIndex + 1);

            _temp.SetPos1();

            for (var i = 0; i < qStatistics.Value.AnswersDistribution.Length; i++)
            {
                _temp.Write(i + 1);
                _temp.WriteLine(qStatistics.Value.AnswersDistribution[i]);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("QuestionStatistics" + qStatistics.Key.GetHashCode(),
                "Ответы пользователей", _questions);
            _questions.WriteLine();
        }

        public void ToFile(string path, DataAnalyserReport report)
		{
            _main.Write("Всего тестов:");
			_main.WriteLine(report.TotalAmountOfTests);

            _main.Write("Количество уникальных e-mail'ов:");
            _main.WriteLine(report.AmountOfUniqueEmails);

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
