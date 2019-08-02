namespace QuizData.Analyser.Models.DataBlocks
{
    public class ScalarDataBlock : IDataBlock
    {
        public object Data { get; }
        public string Caption { get; }

        public ScalarDataBlock(object data, string caption)
        {
            Data = data;
            Caption = caption;
        }
    }
}
