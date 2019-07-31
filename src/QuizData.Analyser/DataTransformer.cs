using QuizData.Analyser.Models;
using QuizData.Analyser.Models.DataBlocks;
using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser
{
    public static class DataTransformer
    {
        public static DistributionDataBlock<uint, TValue> MakeDistributionDataBlock<TValue>
            (IEnumerable<TValue> data, string chartTitle)
        {
            var i = 1U;
            var dictionary = data.ToDictionary(x => i++, x => x);
            return new DistributionDataBlock<uint, TValue>(dictionary, chartTitle);
        }

        public static DistributionDataBlock<string, uint> MakeDistributionDataBlock
            (NumericDistribution distribution, string chartTitle)
        {
            var dictionary = distribution.Intervals.ToDictionary(
                x => string.Format("[{0:F0}; {1:F0})", x.LeftBorder, x.RightBorder),
                x => x.NumericsAmount);
            return new DistributionDataBlock<string, uint>(dictionary, chartTitle);
        }

        public static DoubleDistributionDataBlock<uint, TValue> MakeDoubleDistributionDataBlock<TValue>
            (IEnumerable<TValue> data, uint IntervalsNumber, string chartTitle, string[] axisTitles)
        {
            var list = new List<KeyValuePair<uint, IEnumerable<TValue>>>();
            for (var j = 0U; j < IntervalsNumber; j++)
            {
                var innerList = new List<TValue>((int)IntervalsNumber);
                for (var k = j * IntervalsNumber; k < (j + 1) * IntervalsNumber; k++)
                {
                    innerList.Add(data.ElementAt((int)k));
                }
                list.Add(new KeyValuePair<uint, IEnumerable<TValue>>(j, innerList));
            }
            return new DoubleDistributionDataBlock<uint, TValue>(list, chartTitle,
                axisTitles[0], null,
                axisTitles[1], null,
                axisTitles[2], null);
        }

        public static IEnumerable<IDataBlock> GetMainData(this DataAnalyserReport report)
        {
            var data = new List<IDataBlock>();

            var scalarDB = new ScalarDataBlock(report.TotalAmountOfTests, "Всего тестов:");
            data.Add(scalarDB);

            scalarDB = new ScalarDataBlock(report.AmountOfUniqueEmails,
                "Количество уникальных e-mail'ов:");
            data.Add(scalarDB);

            var i = 0U;
            var attemptDistribution = report.AttemptDistribution
                .ToDictionary(x => ++i, x => x)
                .Where(x => x.Value != 0);

            var distributionDataBlock = new DistributionDataBlock<uint, uint>(attemptDistribution,
                "Распределение попыток");
            data.Add(distributionDataBlock);

            var resultDistribution = report.ResultDistribution.ToList();
            resultDistribution.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            distributionDataBlock = new DistributionDataBlock<uint, uint>(resultDistribution,
                "Распределение результатов");
            data.Add(distributionDataBlock);

            (var kDistr, var bDistr) = report.GetAdditionalInfo();

            if (kDistr != null && bDistr != null)
            {
                data.Add(
                    MakeDistributionDataBlock(kDistr, "Распределение коэффициента K"));

                data.Add(
                    MakeDistributionDataBlock(bDistr, "Распределение коэффициента B"));

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

                data.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.NumericsAmount), distr.IntervalsNumber,
                    "Распределение по K и B",
                    new[] { "K", "Количество человек", "B" }));
                data.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.SigmaMin), distr.IntervalsNumber,
                    "Распределение по K и B SigmaMin",
                    new[] { "K", "SigmaMin", "B" }));
                data.Add(MakeDoubleDistributionDataBlock(
                    distr.Parts.Select(x => x.SigmaMax), distr.IntervalsNumber,
                    "Распределение по K и B SigmaMax",
                    new[] { "K", "SigmaMax", "B" }));
            }

            return data;
        }

        public static IEnumerable<IDataBlock> GetQuestionsData(this DataAnalyserReport report)
        {
            var data = new List<IDataBlock>();

            foreach (var el in report.QuestionStatistics)
            {
                var scalarDB = new ScalarDataBlock(el.Key, "Вопрос:");
                data.Add(scalarDB);

                scalarDB = new ScalarDataBlock(el.Value.RightAnswersAmount, "Правильных ответов:");
                data.Add(scalarDB);

                scalarDB = new ScalarDataBlock(el.Value.WrongAnswersAmount, "Неправильных ответов:");
                data.Add(scalarDB);

                scalarDB = new ScalarDataBlock(el.Value.RightAnswersAmount +
                    el.Value.WrongAnswersAmount, "Всего ответов:");
                data.Add(scalarDB);

                scalarDB = new ScalarDataBlock(el.Value.RightAnswerIndex + 1, "Правильный ответ:");
                data.Add(scalarDB);

                var distributionDB = MakeDistributionDataBlock(
                    el.Value.AnswersDistribution,
                    "Ответы пользователей");
                data.Add(distributionDB);

                scalarDB = new ScalarDataBlock("", "");
                data.Add(scalarDB);
            }

            return data;
        }
    }
}
