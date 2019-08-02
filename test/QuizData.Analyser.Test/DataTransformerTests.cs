using QuizData.Analyser.Models.DataBlocks;
using System.Linq;
using Xunit;

namespace QuizData.Analyser.Test
{
    public class DataTransformerTests
    {
        internal void CompareScalarDataBlocks(ScalarDataBlock expected, ScalarDataBlock actual)
        {
            Assert.Equal(expected.Caption, actual.Caption);
            Assert.Equal(expected.Data, actual.Data);
        }

        internal void CompareDistributionDataBlocks<TKey, TValue>(
            DistributionDataBlock<TKey, TValue> expected,
            DistributionDataBlock<TKey, TValue> actual)
        {
            Assert.Equal(expected.Data, actual.Data);
            Assert.Equal(expected.IntervalValueTitle, actual.IntervalValueTitle);
            Assert.Equal(expected.IntervalValueUoM, actual.IntervalValueUoM);
            Assert.Equal(expected.MeasuredValueTitle, actual.MeasuredValueTitle);
            Assert.Equal(expected.MeasuredValueUoM, actual.MeasuredValueUoM);
            Assert.Equal(expected.Title, actual.Title);
        }

        internal void CompareDoubleDistributionDataBlocks<TKey, TValue>(
            DoubleDistributionDataBlock<TKey, TValue> expected,
            DoubleDistributionDataBlock<TKey, TValue> actual)
        {
            Assert.Equal(expected.Data, actual.Data);
            Assert.Equal(expected.Interval1ValueTitle, actual.Interval1ValueTitle);
            Assert.Equal(expected.Interval1ValueUoM, actual.Interval1ValueUoM);
            Assert.Equal(expected.Interval2ValueTitle, actual.Interval2ValueTitle);
            Assert.Equal(expected.Interval2ValueUoM, actual.Interval2ValueUoM);
            Assert.Equal(expected.MeasuredValueTitle, actual.MeasuredValueTitle);
            Assert.Equal(expected.MeasuredValueUoM, actual.MeasuredValueUoM);
            Assert.Equal(expected.Title, actual.Title);
        }

        internal void CompareDataBlocks(IDataBlock expected, IDataBlock actual)
        {
            if (expected is ScalarDataBlock expectedScalar &&
                actual is ScalarDataBlock actualScalar)
            {
                CompareScalarDataBlocks(expectedScalar, actualScalar);
            }
            else if (expected is DistributionDataBlock<string, uint> expectedDistribution &&
                actual is DistributionDataBlock<string, uint> actualDistribution)
            {
                CompareDistributionDataBlocks(expectedDistribution, actualDistribution);
            }
            else if (expected is DistributionDataBlock<uint, uint> expectedDistribution2 &&
                actual is DistributionDataBlock<uint, uint> actualDistribution2)
            {
                CompareDistributionDataBlocks(expectedDistribution2, actualDistribution2);
            }
            else if (expected is DoubleDistributionDataBlock<uint, uint> expectedDoubleDistribution &&
                actual is DoubleDistributionDataBlock<uint, uint> actualDoubleDistribution)
            {
                CompareDoubleDistributionDataBlocks(expectedDoubleDistribution, actualDoubleDistribution);
            }
            else if (expected is DoubleDistributionDataBlock<uint, double?> expectedDoubleDistribution2 &&
                actual is DoubleDistributionDataBlock<uint, double?> actualDoubleDistribution2)
            {
                CompareDoubleDistributionDataBlocks(expectedDoubleDistribution2, actualDoubleDistribution2);
            }
            else
            {
                throw new System.ArgumentException("DataBlock wasn't recognized");
            }
        }

        [Fact]
        public void DataTransformerTest()
        {
            var mainData = Resources.DataAnalyserReport.GetMainData();
            var questionsData = Resources.DataAnalyserReport.GetQuestionsData();

            Assert.Equal(Resources.MainData.Count(), mainData.Count());
            for (var i = 0; i < mainData.Count(); i++)
            {
                CompareDataBlocks(Resources.MainData.ElementAt(i), mainData.ElementAt(i));
            }

            Assert.Equal(Resources.QuestionsData.Count(), questionsData.Count());
            for (var i = 0; i < questionsData.Count(); i++)
            {
                CompareDataBlocks(Resources.QuestionsData.ElementAt(i), questionsData.ElementAt(i));
            }
        }
    }
}
