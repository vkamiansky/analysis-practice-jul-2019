using System.Collections.Generic;

namespace QuizData.Analyser.Models.DataBlocks
{
    public class DoubleDistributionDataBlock<TKey, TValue> : IDataBlock
    {
        public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> Data { get; }
        public string Title { get; }
        public string Interval1ValueTitle { get; }
        public string Interval1ValueUoM { get; }
        public string Interval2ValueTitle { get; }
        public string Interval2ValueUoM { get; }
        public string MeasuredValueTitle { get; }
        public string MeasuredValueUoM { get; }

        public DoubleDistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> data)
        {
            Data = data ?? throw new System.ArgumentNullException("Data can not be null");
        }

        public DoubleDistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> data,
            string title)
            : this(data)
        {
            Title = title;
        }

        public DoubleDistributionDataBlock(
            IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> data,
            string title,
            string interval1ValueTitle,
            string interval1ValueUoM,
            string interval2ValueTitle,
            string interval2ValueUoM,
            string measuredValueTitle,
            string measuredValueUoM)
            : this(data, title)
        {
            Interval1ValueTitle = interval1ValueTitle;
            Interval1ValueUoM = interval1ValueUoM;
            Interval2ValueTitle = interval2ValueTitle;
            Interval2ValueUoM = interval2ValueUoM;
            MeasuredValueTitle = measuredValueTitle;
            MeasuredValueUoM = measuredValueUoM;
        }
    }
}
