namespace QuizData.ExcelReport
{
    public class ScalarDataBlock : IDataBlock
    {
        private readonly object _data;
        private readonly string _caption;

        public ScalarDataBlock(object data, string caption)
        {
            _data = data;
            _caption = caption;
        }
    }
}
