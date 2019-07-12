namespace QuizData.Analyser.Models
{
	public class QuestionStatistics
	{
		public uint RightAnswersAmount { get; set; }
		public uint WrongAnswersAmount { get; set; }
		public uint RightAnswerIndex { get; set; }
		public uint[] AnswersDistribution { get; set; }

		public QuestionStatistics()
		{
			RightAnswersAmount = 0;
			WrongAnswersAmount = 0;
			RightAnswerIndex = 0;
			AnswersDistribution = new uint[4];
		}
	}
}
