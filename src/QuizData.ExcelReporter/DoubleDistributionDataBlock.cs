using System.Collections.Generic;

namespace QuizData.ExcelReport
{
    public class DoubleDistributionDataBlock<TKey, TValue> : IDataBlock
    {
        public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> Distribution { get; }
        public string ChartName { get; }
        public string ChartTitle { get; }
        public string[] AxisTitles { get; }

        public DoubleDistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> distribution,
            string chartName,
            string chartTitle,
            string[] axisTitles)
        {
            Distribution = distribution;
            ChartName = chartName;
            ChartTitle = chartTitle;
            AxisTitles = axisTitles;
        }
    }
}
