using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using QuizData.Analyser.Models;
using System.Collections.Generic;
using OfficeOpenXml.Drawing.Chart;

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

        public void BuildKAndBDistributionsChart(DataAnalyserReport report)
        {
            (var kDistr, var bDistr) = report.GetAdditionalInfo();

            // K Distribution
            _temp.SetPos1();

            foreach (var el in kDistr.Parts)
            {
                _temp.Write(string.Format("[{0:F2}; {1:F2})", el.LeftBorder, el.RightBorder));
                _temp.WriteLine(el.NumericsAmount);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("KDistribution", "Распределение коэффициента K", _main);

            // B Distribution
            _temp.SetPos1();

            foreach (var el in bDistr.Parts)
            {
                _temp.Write(string.Format("[{0:F2}; {1:F2})", el.LeftBorder, el.RightBorder));
                _temp.WriteLine(el.NumericsAmount);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("BDistribution", "Распределение коэффициента B", _main);
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
            BuildKAndBDistributionsChart(report);

            foreach (var el in report.QuestionStatistics)
                Question(el);

            (var kDistr, var bDistr) = report.GetAdditionalInfo();

            var distr = new DoubleNumericDistribution(kDistr.LeftBorder, kDistr.RightBorder,
                bDistr.LeftBorder, bDistr.RightBorder, 10);

            foreach (var el in report.PersonStatistics)
            {
                if (el.Value.AdditionalInfo != null)
                {
                    distr.AddNumerics(el.Value.AdditionalInfo.Value.K, el.Value.AdditionalInfo.Value.B);
                }
            }



            _temp.WriteLine();
            _temp.WriteLine();
            _temp.WriteLine();

            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }

            _temp.WriteLine();

            _temp.Write();
            _temp.SetPos1();
            _temp.GoBack();

            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
                for (var j = (i - 1) * 10; j < (i - 1) * 10 + 10; j++)
                {
                    _temp.Write(distr.Parts.ElementAt(j).NumericsAmount);
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();

            var chart = _main.Worksheet.Drawings.AddChart("WhatIsItSurface", eChartType.Surface);
            chart.Title.Text = "Распределение по K и B";
            for (var i = 82; i < 92; i++)
            {
                var address1 = string.Format("B{0}:K{0}", i);
                var address2 = "A82:A91";
                chart.Series.Add(ExcelRange.GetFullAddress(_temp.Worksheet.Name, address1),
                    ExcelRange.GetFullAddress(_temp.Worksheet.Name, address2));
            }
            chart.Legend.Position = eLegendPosition.Right;

            _main.PlaceChart(chart);





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
