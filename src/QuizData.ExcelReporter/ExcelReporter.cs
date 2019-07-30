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

        #region Work with DataBlocks

        public void WriteDataBlock(IDataBlock dataBlock, ExcelWorksheetWrapper to)
        {
            if (dataBlock is ScalarDataBlock scalarDataBlock)
            {
                WriteScalarDataBlock(scalarDataBlock, to);
            }
            else if (dataBlock is DistributionDataBlock<string, uint> doubleDistributionDataBlock)
            {
                WriteDistributionDataBlock(doubleDistributionDataBlock, to);
            }
            else if (dataBlock is DistributionDataBlock<uint, uint> uintDistributionDataBlock)
            {
                WriteDistributionDataBlock(uintDistributionDataBlock, to);
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, uint> c)
            {
                WriteDoubleDistributionDataBlock(c, to);
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, double?> d)
            {
                WriteDoubleDistributionDataBlock(d, to);
            }
            else
            {
                throw new System.ArgumentException("DataBlock wasn't recognized");
            }
        }

        public void WriteDataBlocks(IEnumerable<IDataBlock> dataBlocks, ExcelWorksheetWrapper to)
        {
            foreach (var dataBlock in dataBlocks)
                WriteDataBlock(dataBlock, to);
        }

        public void WriteScalarDataBlock(ScalarDataBlock dataBlock, ExcelWorksheetWrapper to)
        {
            to.Write(dataBlock.Caption);
            to.WriteLine(dataBlock.Data);
        }

        public void WriteDistributionDataBlock<TKey, TValue>(DistributionDataBlock<TKey, TValue> db, ExcelWorksheetWrapper to)
        {
            _temp.SetPos1();

            foreach (var el in db.Distribution)
            {
                _temp.Write(el.Key);
                _temp.WriteLine(el.Value);
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.CreateChart(db.ChartName, db.ChartTitle, to);
        }

        public void WriteDoubleDistributionDataBlock<TKey, TValue>(DoubleDistributionDataBlock<TKey, TValue> db, ExcelWorksheetWrapper to)
        {
            _temp.SetPos1();

            for (var i = 0; i < db.Distribution.Count(); i++)
            {
                var current = db.Distribution.ElementAt(i);
                _temp.Write(current.Key);
                for (var j = 0; j < current.Value.Count(); j++)
                {
                    _temp.Write(current.Value.ElementAt(j));
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            _temp.Create3DChart(db.ChartName, db.ChartTitle, to, db.AxisTitles);
        }

        #endregion

        public void BuildQuestionStatistics(List<IDataBlock> dataBlocks, KeyValuePair<string, QuestionStatistics> qStatistics)
        {
            var scalarDB = new ScalarDataBlock(qStatistics.Key, "Вопрос:");
            dataBlocks.Add(scalarDB);

            scalarDB = new ScalarDataBlock(qStatistics.Value.RightAnswersAmount, "Правильных ответов:");
            dataBlocks.Add(scalarDB);

            scalarDB = new ScalarDataBlock(qStatistics.Value.WrongAnswersAmount, "Неправильных ответов:");
            dataBlocks.Add(scalarDB);

            scalarDB = new ScalarDataBlock(qStatistics.Value.RightAnswersAmount +
                qStatistics.Value.WrongAnswersAmount, "Всего ответов:");
            dataBlocks.Add(scalarDB);

            scalarDB = new ScalarDataBlock(qStatistics.Value.RightAnswerIndex + 1, "Правильный ответ:");
            dataBlocks.Add(scalarDB);

            var distributionDB = MakeDistributionDataBlock(
                qStatistics.Value.AnswersDistribution,
                "QuestionStatistics-" + qStatistics.Key.GetHashCode(),
                "Ответы пользователей");
            dataBlocks.Add(distributionDB);

            scalarDB = new ScalarDataBlock("", "");
            dataBlocks.Add(scalarDB);
        }

        public DistributionDataBlock<uint, uint> MakeDistributionDataBlock
            (IEnumerable<uint> data, string chartName, string chartTitle)
        {
            var i = 1U;
            var dictionary = data.ToDictionary(x => i++, x => x);
            return new DistributionDataBlock<uint, uint>(dictionary, chartName, chartTitle);
        }

        public DistributionDataBlock<string, uint> MakeDistributionDataBlock
            (NumericDistribution distribution, string chartName, string chartTitle)
        {
            var dictionary = distribution.Intervals.ToDictionary(
                x => string.Format("[{0:F0}; {1:F0})", x.LeftBorder, x.RightBorder),
                x => x.NumericsAmount);
            return new DistributionDataBlock<string, uint>(dictionary, chartName, chartTitle);
        }

        public DoubleDistributionDataBlock<uint, TValue> MakeDoubleDistributionDataBlock<TValue>
            (IEnumerable<TValue> data, uint IntervalsAmount, string chartName, string chartTitle, string[] axisTitles)
        {
            var list = new List<KeyValuePair<uint, IEnumerable<TValue>>>();
            for (var j = 0U; j < IntervalsAmount; j++)
            {
                var innerList = new List<TValue>((int)IntervalsAmount);
                for (var k = j * IntervalsAmount; k < (j + 1) * IntervalsAmount; k++)
                {
                    innerList.Add(data.ElementAt((int)k));
                }
                list.Add(new KeyValuePair<uint, IEnumerable<TValue>>(j, innerList));
            }
            return new DoubleDistributionDataBlock<uint, TValue>(list, chartName, chartTitle, axisTitles);
        }

        public void ToStream(Stream stream, DataAnalyserReport report)
        {
            var mainDataBlocks = new List<IDataBlock>();

            var scalarDB = new ScalarDataBlock(report.TotalAmountOfTests, "Всего тестов:");
            mainDataBlocks.Add(scalarDB);

            scalarDB = new ScalarDataBlock(report.AmountOfUniqueEmails,
                "Количество уникальных e-mail'ов:");
            mainDataBlocks.Add(scalarDB);

            var i = 0U;
            var attemptDistribution = report.AttemptDistribution
                .ToDictionary(x => ++i, x => x)
                .Where(x => x.Value != 0);

            var distributionDataBlock = new DistributionDataBlock<uint, uint>(attemptDistribution,
                "AttemptDistribution", "Распределение попыток");
            mainDataBlocks.Add(distributionDataBlock);

            var resultDistribution = report.ResultDistribution.ToList();
            resultDistribution.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            distributionDataBlock = new DistributionDataBlock<uint, uint>(resultDistribution,
                "ResultDistribution", "Распределение результатов");
            mainDataBlocks.Add(distributionDataBlock);

            (var kDistr, var bDistr) = report.GetAdditionalInfo();

            if (kDistr != null && bDistr != null)
            {
                mainDataBlocks.Add(
                    MakeDistributionDataBlock(kDistr, "KDistribution", "Распределение коэффициента K"));

                mainDataBlocks.Add(
                    MakeDistributionDataBlock(bDistr, "BDistribution", "Распределение коэффициента B"));

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

                mainDataBlocks.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.NumericsAmount), distr.IntervalsAmount,
                    "KnBDistribution", "Распределение по K и B",
                    new[] { "K", "Количество человек", "B" }));
                mainDataBlocks.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.SigmaMin), distr.IntervalsAmount,
                    "SigmaMinDistribution", "Распределение по K и B SigmaMin",
                    new[] { "K", "Количество человек", "B" }));
                mainDataBlocks.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.SigmaMax), distr.IntervalsAmount,
                    "SigmaMaxDistribution", "Распределение по K и B SigmaMax",
                    new[] { "K", "SigmaMax", "B" }));
            }

            var questionsDataBlocks = new List<IDataBlock>();

            foreach (var el in report.QuestionStatistics)
                BuildQuestionStatistics(questionsDataBlocks, el);

            WriteDataBlocks(mainDataBlocks, _main);
            WriteDataBlocks(questionsDataBlocks, _questions);

            _package.SaveAs(stream);
        }
    }
}
