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

        public void BuildDistributionChart(NumericDistribution distribution)
        {
            _temp.SetPos1();

            foreach (var el in distribution.Parts)
            {
                _temp.Write(string.Format("[{0:F2}; {1:F2})", el.LeftBorder, el.RightBorder));
                _temp.WriteLine(el.NumericsAmount);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("Distribution-" + distribution.GetHashCode(),
                "Распределение коэффициента K", _main);
        }

        public void BuildKAndBDistributionsChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }

            _temp.WriteLine();

            _temp.Write();

            var dataStartLine = _temp.CurrentLine;
            var dataStartColumn = _temp.CurrentColumn;

            _temp.GoBack();

            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
                for (var j = (i - 1) * 10; j < (i - 1) * 10 + 10; j++)
                {
                    _temp.Write(distribution.Parts.ElementAt(j).NumericsAmount);
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();

            var chart = _main.Worksheet.Drawings.AddChart("WhatIsItSurface", eChartType.Surface);
            chart.Title.Text = "Распределение по K и B";
            for (var i = dataStartLine; i <= _temp.CurrentLine; i++)
            {
                var address1 = string.Format("B{0}:K{0}", i);
                var address2 = string.Format("A{0}:A{1}", dataStartLine, _temp.CurrentLine);
                var serie = chart.Series.Add(ExcelRange.GetFullAddress(_temp.Worksheet.Name, address1),
                    ExcelRange.GetFullAddress(_temp.Worksheet.Name, address2));
                serie.Header = _temp.Worksheet.Cells[string.Format("A{0}", i)].Value.ToString();
            }
            chart.Legend.Position = eLegendPosition.Right;
            chart.XAxis.Title.Text = "K";
            chart.YAxis.Title.Text = "Количество человек";
            chart.Axis[2].Title.Text = "B";

            foreach (var axis in chart.Axis)
                axis.Title.Font.Size = 12;

            _main.PlaceChart(chart, 20);

            _temp.WriteLine();
        }

        public void BuildSigmaMinDistributionsChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }

            _temp.WriteLine();

            _temp.Write();

            var dataStartLine = _temp.CurrentLine;
            var dataStartColumn = _temp.CurrentColumn;

            _temp.GoBack();

            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
                for (var j = (i - 1) * 10; j < (i - 1) * 10 + 10; j++)
                {
                    _temp.Write(distribution.Parts.ElementAt(j).SigmaMin);
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();

            var chart = _main.Worksheet.Drawings.AddChart("WhatIsItSurfaceSM", eChartType.Surface);
            chart.Title.Text = "Распределение по K и B SigmaMin";
            for (var i = dataStartLine; i <= _temp.CurrentLine; i++)
            {
                var address1 = string.Format("B{0}:K{0}", i);
                var address2 = string.Format("A{0}:A{1}", dataStartLine, _temp.CurrentLine);
                var serie = chart.Series.Add(ExcelRange.GetFullAddress(_temp.Worksheet.Name, address1),
                    ExcelRange.GetFullAddress(_temp.Worksheet.Name, address2));
                serie.Header = _temp.Worksheet.Cells[string.Format("A{0}", i)].Value.ToString();
            }
            chart.Legend.Position = eLegendPosition.Right;
            chart.XAxis.Title.Text = "K";
            chart.YAxis.Title.Text = "SigmaMin";
            chart.Axis[2].Title.Text = "B";

            foreach (var axis in chart.Axis)
                axis.Title.Font.Size = 12;

            _main.PlaceChart(chart, 20);

            _temp.WriteLine();
        }

        public void BuildSigmaMaxDistributionsChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }

            _temp.WriteLine();

            _temp.Write();

            var dataStartLine = _temp.CurrentLine;
            var dataStartColumn = _temp.CurrentColumn;

            _temp.GoBack();

            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
                for (var j = (i - 1) * 10; j < (i - 1) * 10 + 10; j++)
                {
                    _temp.Write(distribution.Parts.ElementAt(j).SigmaMax);
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();

            var chart = _main.Worksheet.Drawings.AddChart("WhatIsItSurfaceSMax", eChartType.Surface);
            chart.Title.Text = "Распределение по K и B SigmaMax";
            for (var i = dataStartLine; i <= _temp.CurrentLine; i++)
            {
                var address1 = string.Format("B{0}:K{0}", i);
                var address2 = string.Format("A{0}:A{1}", dataStartLine, _temp.CurrentLine);
                var serie = chart.Series.Add(ExcelRange.GetFullAddress(_temp.Worksheet.Name, address1),
                    ExcelRange.GetFullAddress(_temp.Worksheet.Name, address2));
                serie.Header = _temp.Worksheet.Cells[string.Format("A{0}", i)].Value.ToString();
            }
            chart.Legend.Position = eLegendPosition.Right;
            chart.XAxis.Title.Text = "K";
            chart.YAxis.Title.Text = "SigmaMax";
            chart.Axis[2].Title.Text = "B";

            foreach (var axis in chart.Axis)
                axis.Title.Font.Size = 12;

            _main.PlaceChart(chart, 20);

            _temp.WriteLine();
        }

        public void BuildQuestionStatistics(KeyValuePair<string, QuestionStatistics> qStatistics)
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
            _temp.CreateChart("QuestionStatistics-" + qStatistics.Key.GetHashCode(),
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

            (var kDistr, var bDistr) = report.GetAdditionalInfo();
            BuildDistributionChart(kDistr);
            BuildDistributionChart(bDistr);

            var distr = new DoubleNumericDistribution(kDistr.LeftBorder, kDistr.RightBorder,
                bDistr.LeftBorder, bDistr.RightBorder, 10);

            foreach (var el in report.PersonStatistics)
            {
                if (el.Value.AdditionalInfo != null)
                {
                    distr.AddNumerics(el.Value.AdditionalInfo.Value.K,
                        el.Value.AdditionalInfo.Value.B,
                        el.Value.AdditionalInfo.Value.R);
                }
            }

            BuildKAndBDistributionsChart(distr);
            BuildSigmaMinDistributionsChart(distr);
            BuildSigmaMaxDistributionsChart(distr);

            foreach (var el in report.QuestionStatistics)
                BuildQuestionStatistics(el);

            _package.SaveAs(new FileInfo(path));
		}
	}
}
