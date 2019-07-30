using System.Collections.Generic;

namespace QuizData.ExcelReport
{
    public class DistributionDataBlock<TKey, TValue> : IDataBlock
    {
        public IEnumerable<KeyValuePair<TKey, TValue>> Distribution { get; }
        public string ChartName { get; }
        public string ChartTitle { get; }

        public DistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, TValue>> distribution,
            string chartName,
            string chartTitle)
        {
            Distribution = distribution;
            ChartName = chartName;
            ChartTitle = chartTitle;
        }
    }
}
