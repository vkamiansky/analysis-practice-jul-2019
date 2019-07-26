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

        public void BuildAttemptDistributionChart(uint[] attemptDistribution)
        {
            _temp.SetPos1();

            for (var i = 0; i < attemptDistribution.Length; i++)
            {
                if (attemptDistribution[i] != 0)
                {
                    _temp.Write(i + 1);
                    _temp.WriteLine(attemptDistribution[i]);
                }
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart("AttemptDistribution", "Распределение попыток", _main);
        }

        public void BuildResultDistributionChart(Dictionary<uint, uint> resultDistribution)
        {
            _temp.SetPos1();

            var list = resultDistribution.ToList();
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

        public void BuildNumericDistributionChart(NumericDistribution distribution,
            string chartName, string chartTitle)
        {
            _temp.SetPos1();

            foreach (var el in distribution.Intervals)
            {
                _temp.Write(string.Format("[{0:F2}; {1:F2})", el.LeftBorder, el.RightBorder));
                _temp.WriteLine(el.NumericsAmount);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart(chartName, chartTitle, _main);
        }

        public void BuildKAndBDistributionsChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }
            _temp.WriteLine();

            _temp.SetPos1();

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

            _temp.Create3DChart("KnBDistribution", "Распределение по K и B",
                _main, new[] { "K", "Количество человек", "B" });

            _temp.WriteLine();
        }

        public void BuildSigmaMinDistributionChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }
            _temp.WriteLine();

            _temp.SetPos1();

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

            _temp.Create3DChart("SigmaMinDistribution", "Распределение по K и B SigmaMin",
                _main, new[] { "K", "SigmaMin", "B" });

            _temp.WriteLine();
        }

        public void BuildSigmaMaxDistributionChart(DoubleNumericDistribution distribution)
        {
            _temp.Write("");
            for (var i = 1; i < 11; i++)
            {
                _temp.Write(i);
            }
            _temp.WriteLine();

            _temp.SetPos1();

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

            _temp.Create3DChart("SigmaMaxDistribution", "Распределение по K и B SigmaMax",
                _main, new[] { "K", "SigmaMax", "B" });

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

        public void ToStream(Stream stream, DataAnalyserReport report)
        {
            _main.Write("Всего тестов:");
            _main.WriteLine(report.TotalAmountOfTests);

            _main.Write("Количество уникальных e-mail'ов:");
            _main.WriteLine(report.AmountOfUniqueEmails);

            BuildAttemptDistributionChart(report.AttemptDistribution);
            BuildResultDistributionChart(report.ResultDistribution);

            (var kDistr, var bDistr) = report.GetAdditionalInfo();
            BuildNumericDistributionChart(kDistr, "KDistribution", "Распределение коэффициента K");
            BuildNumericDistributionChart(bDistr, "BDistribution", "Распределение коэффициента B");

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
            BuildSigmaMinDistributionChart(distr);
            BuildSigmaMaxDistributionChart(distr);

            foreach (var el in report.QuestionStatistics)
                BuildQuestionStatistics(el);

            _package.SaveAs(stream);
        }
    }
}
