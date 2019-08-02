using System.Collections.Generic;

namespace QuizData.Analyser.Models.DataBlocks
{
    public class DistributionDataBlock<TKey, TValue> : IDataBlock
    {
        public IEnumerable<KeyValuePair<TKey, TValue>>[] Data { get; }
        public string Title { get; }
        public string IntervalValueTitle { get; }
        public string IntervalValueUoM { get; }
        public string MeasuredValueTitle { get; }
        public string MeasuredValueUoM { get; }

        public DistributionDataBlock(IEnumerable<KeyValuePair<TKey, TValue>>[] data)
        {
            Data = data ?? throw new System.ArgumentNullException("Data can not be null");
        }

        public DistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, TValue>>[] data,
            string title)
            : this(data)
        {
            Title = title;
        }

        public DistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, TValue>>[] data,
            string title,
            string intervalValueTitle,
            string intervalValueUoM,
            string measuredValueTitle,
            string measuredValueUoM)
            : this(data, title)
        {
            IntervalValueTitle = intervalValueTitle;
            IntervalValueUoM = intervalValueUoM;
            MeasuredValueTitle = measuredValueTitle;
            MeasuredValueUoM = measuredValueUoM;
        }
    }
}
