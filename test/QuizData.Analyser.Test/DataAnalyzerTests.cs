using QuizData.Parser;
using Xunit;
using QuizData.Analyser.Models;

namespace QuizData.Analyser.Test
{
	public class DataAnalyzerTests
	{
		internal void CompareQuestionStatistics(QuestionStatistics expected, QuestionStatistics actual)
		{
			Assert.Equal(expected.AnswersDistribution, actual.AnswersDistribution);
			Assert.Equal(expected.RightAnswerIndex, actual.RightAnswerIndex);
			Assert.Equal(expected.RightAnswersAmount, actual.RightAnswersAmount);
			Assert.Equal(expected.WrongAnswersAmount, actual.WrongAnswersAmount);
		}

		[Fact]
		public void TestOneQuiz()
		{
			var parser = new CsvParser();
			var data = parser.ParseFile("data/testOneQuiz.txt");
			var results = DataAnalyser.Analyze(data);

			Assert.Equal(1U, results.AmountOfUniqueEmails);

			var attemptDistribution = new uint[] { 1 };
			Assert.Equal(attemptDistribution, results.AttemptDistribution);

			var questions = new[]
			{
				"Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?",
				"Багет - это хлеб, а хлеб ? это багет?",
				"Путник знает, что и как заказать в таверне.",
				"Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?",
				"Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?"
			};

			var qStatistics = new QuestionStatistics[]
			{
				new QuestionStatistics
				{
					AnswersDistribution = new uint[] {0, 0, 1, 0},
					RightAnswerIndex = 2,
					RightAnswersAmount = 1,
					WrongAnswersAmount = 0
				},
				new QuestionStatistics
				{
					AnswersDistribution = new uint[] {0, 0, 0, 1},
					RightAnswerIndex = 0,
					RightAnswersAmount = 0,
					WrongAnswersAmount = 1
				},
				new QuestionStatistics
				{
					AnswersDistribution = new uint[] {1, 0, 0, 0},
					RightAnswerIndex = 0,
					RightAnswersAmount = 1,
					WrongAnswersAmount = 0
				},
				new QuestionStatistics
				{
					AnswersDistribution = new uint[] {0, 0, 1, 0},
					RightAnswerIndex = 2,
					RightAnswersAmount = 1,
					WrongAnswersAmount = 0
				},
				new QuestionStatistics
				{
					AnswersDistribution = new uint[] {1, 0, 0, 0},
					RightAnswerIndex = 2,
					RightAnswersAmount = 0,
					WrongAnswersAmount = 1
				}
			};

			Assert.Equal(5, results.QuestionStatistics.Count);
			for (var i = 0; i < questions.Length; i++)
			{
				Assert.True(results.QuestionStatistics.ContainsKey(questions[i]));
				CompareQuestionStatistics(qStatistics[i], results.QuestionStatistics[questions[i]]);
			}

			Assert.Single(results.ResultDistribution);
			Assert.True(results.ResultDistribution.ContainsKey(60U));
			Assert.Equal(1U, results.ResultDistribution[60U]);

			Assert.Equal(1U, results.TotalAmountOfTests);
		}
	}
}
