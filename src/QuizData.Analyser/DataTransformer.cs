using QuizData.Analyser.Models;
using QuizData.Analyser.Models.DataBlocks;
using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser
{
    public static class DataTransformer
    {
        public static DistributionDataBlock<uint, TValue> MakeDistributionDataBlock<TValue>
            (string title, IEnumerable<TValue> data)
        {
            var i = 1U;
            var dictionary = data.ToDictionary(x => i++, x => x);
            return new DistributionDataBlock<uint, TValue>
                (new IEnumerable<KeyValuePair<uint, TValue>>[] { dictionary }, title);
        }

        public static DistributionDataBlock<string, uint> MakeDistributionDataBlock
            (string title, NumericDistribution[] distributions)
        {
            var data = new Dictionary<string, uint>[distributions.Length];
            for (var i = 0; i < distributions.Length; i++)
            {
                data[i] = distributions[i].Intervals.ToDictionary(
                    x => string.Format("[{0:F0}; {1:F0})", x.LeftBorder, x.RightBorder),
                    x => x.NumericsAmount);
            }
            return new DistributionDataBlock<string, uint>(data, title);
        }

        public static DistributionDataBlock<string, uint> MakeDistributionDataBlock
            (string title, NumericDistribution distribution)
        {
            return MakeDistributionDataBlock(title, new[] { distribution });
        }

        public static DoubleDistributionDataBlock<uint, TValue> MakeDoubleDistributionDataBlock<TValue>
            (string title, IEnumerable<TValue> data, uint intervalsNumber, string interval1ValueTitle,
            string interval2ValueTitle, string measuredValueTitle)
        {
            var list = new List<KeyValuePair<uint, IEnumerable<TValue>>>();
            for (var j = 0U; j < intervalsNumber; j++)
            {
                var innerList = new List<TValue>((int)intervalsNumber);
                for (var k = j * intervalsNumber; k < (j + 1) * intervalsNumber; k++)
                {
                    innerList.Add(data.ElementAt((int)k));
                }
                list.Add(new KeyValuePair<uint, IEnumerable<TValue>>(j, innerList));
            }
            return new DoubleDistributionDataBlock<uint, TValue>(list, title,
                interval1ValueTitle, null,
                interval2ValueTitle, null,
                measuredValueTitle, null);
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

            var distributionDataBlock = new DistributionDataBlock<uint, uint>
                (new IEnumerable<KeyValuePair<uint, uint>>[] { attemptDistribution },
                "Распределение попыток");
            data.Add(distributionDataBlock);

            var resultDistribution = report.ResultDistribution.ToList();
            resultDistribution.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            distributionDataBlock = new DistributionDataBlock<uint, uint>
                (new IEnumerable<KeyValuePair<uint, uint>>[] { resultDistribution },
                "Распределение результатов");
            data.Add(distributionDataBlock);

            (var kDistr, var bDistr) = report.GetAdditionalInfo();

            if (kDistr != null && bDistr != null)
            {
                data.Add(
                    MakeDistributionDataBlock("Распределение коэффициента K", kDistr));

                data.Add(
                    MakeDistributionDataBlock("Распределение коэффициента B", bDistr));

                data.Add(
                    MakeDistributionDataBlock("Распределение K и B на одной диаграмме",
                    new[] { kDistr, bDistr }));

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
                    "Распределение по K и B",
                    distr.Parts.Select(x => x.NumericsAmount), distr.IntervalsNumber,
                    "K", "Количество человек", "B" ));
                data.Add(MakeDoubleDistributionDataBlock(
                    "Распределение по K и B SigmaMin",
                    distr.Parts.Select(x => x.SigmaMin), distr.IntervalsNumber,
                    "K", "SigmaMin", "B" ));
                data.Add(MakeDoubleDistributionDataBlock(
                    "Распределение по K и B SigmaMax",
                    distr.Parts.Select(x => x.SigmaMax), distr.IntervalsNumber,
                    "K", "SigmaMax", "B" ));
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
                    "Ответы пользователей", el.Value.AnswersDistribution);
                data.Add(distributionDB);

                scalarDB = new ScalarDataBlock("", "");
                data.Add(scalarDB);
            }

            return data;
        }
    }
}
