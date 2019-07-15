using Xunit;
using QuizData.Analyser.Models;
using QuizData.Parser.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser.Test
{
    public class DataAnalyserTests
    {
		private PersonTestResult _personTestResult { get; } = new PersonTestResult()
		{
			Person = new Person
			{
				Name = "Иван",
				Email = "email@site.com"
			},
			Answers = new List<Answer>()
			{
				new Answer
				{
					Question = new Question
					{
						QuestionText = "Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?",
						AnswersList = new List<string>
					{
						"bread bread burger cheese",
						"bread burger cheese bread",
						"bread burger bread cheese",
						"burger cheese bread bread"
					},
						CorrectAnswerIndex = 2
					},
					AnswerIndex = 2
				},
				new Answer
				{
					Question = new Question
					{
						QuestionText = "Багет - это хлеб, а хлеб ? это багет?",
						AnswersList = new List<string>
					{
						"undefined true",
						"false true",
						"true true",
						"true undefined"
					},
						CorrectAnswerIndex = 0
					},
					AnswerIndex = 3
				},
				new Answer
				{
					Question = new Question
					{
						QuestionText = "Путник знает, что и как заказать в таверне.",
						AnswersList = new List<string>
					{
						"[ 'bacon and eggs' ]",
						"[ 'bacon and eggs', 'coffee' ]",
						"[ 'coffee', 'bacon and eggs' ]",
						"SyntaxError: Unexpected token"
					},
						CorrectAnswerIndex = 0
					},
					AnswerIndex = 0
				},
				new Answer
				{
					Question = new Question
					{
						QuestionText = "Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?",
						AnswersList = new List<string>
					{
						"'1-23-4'",
						"-2",
						"'1-2-1'",
						"'-13-4'"
					},
						CorrectAnswerIndex = 2
					},
					AnswerIndex = 2
				},
				new Answer
				{
					Question = new Question
					{
						QuestionText = "Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?",
						AnswersList = new List<string>
					{
						"focus pocus",
						"focus",
						"ничего",
						"undefined"
					},
						CorrectAnswerIndex = 2
					},
					AnswerIndex = 0
				}
			},
			Result = 60,
			Notes = "empty notes"
		};

		private DataAnalyserReport _dataAnalyserReport { get; } = new DataAnalyserReport
		{
			TotalAmountOfTests = 1U,
			PersonStatistics = new Dictionary<string, PersonStatistics>()
			{
				{ "email@site.com", new PersonStatistics()
					{
						AmountOfAttempts = 1,
						Results = new List<uint>()
						{
							60U
						}
					}
				}
			},
			ResultDistribution = new Dictionary<uint, uint>()
				{
					{ 60U, 1U }
				},
			QuestionStatistics = new Dictionary<string, QuestionStatistics>()
				{
					{ "Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?",
					new QuestionStatistics
					{
						AnswersDistribution = new uint[] {0, 0, 1, 0},
						RightAnswerIndex = 2,
						RightAnswersAmount = 1,
						WrongAnswersAmount = 0
					}
					},
					{ "Багет - это хлеб, а хлеб ? это багет?",
					new QuestionStatistics
					{
						AnswersDistribution = new uint[] {0, 0, 0, 1},
						RightAnswerIndex = 0,
						RightAnswersAmount = 0,
						WrongAnswersAmount = 1
					}
					},
					{ "Путник знает, что и как заказать в таверне.",
					new QuestionStatistics
					{
						AnswersDistribution = new uint[] {1, 0, 0, 0},
						RightAnswerIndex = 0,
						RightAnswersAmount = 1,
						WrongAnswersAmount = 0
					}
					},
					{ "Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?",
					new QuestionStatistics
					{
						AnswersDistribution = new uint[] {0, 0, 1, 0},
						RightAnswerIndex = 2,
						RightAnswersAmount = 1,
						WrongAnswersAmount = 0
					}
					},
					{ "Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?",
					new QuestionStatistics
					{
						AnswersDistribution = new uint[] {1, 0, 0, 0},
						RightAnswerIndex = 2,
						RightAnswersAmount = 0,
						WrongAnswersAmount = 1
					}
					}
				}
		};

        internal void CompareQuestionStatistics(QuestionStatistics expected, QuestionStatistics actual)
        {
            Assert.Equal(expected.AnswersDistribution, actual.AnswersDistribution);
            Assert.Equal(expected.RightAnswerIndex, actual.RightAnswerIndex);
            Assert.Equal(expected.RightAnswersAmount, actual.RightAnswersAmount);
            Assert.Equal(expected.WrongAnswersAmount, actual.WrongAnswersAmount);
        }

		internal void ComparePersonStatistics(PersonStatistics expected, PersonStatistics actual)
		{
			Assert.Equal(expected.AmountOfAttempts, actual.AmountOfAttempts);
			Assert.Equal(expected.Results, actual.Results);
		}

		internal void CompareDataAnalyserReports(DataAnalyserReport expected, DataAnalyserReport actual)
		{
			Assert.Equal(expected.TotalAmountOfTests, actual.TotalAmountOfTests);

			Assert.Equal(expected.PersonStatistics.Count, actual.PersonStatistics.Count);
			foreach (var key in expected.PersonStatistics.Keys)
			{
				Assert.True(actual.PersonStatistics.ContainsKey(key));
				ComparePersonStatistics(expected.PersonStatistics[key],
					actual.PersonStatistics[key]);
			}

			Assert.Equal(expected.ResultDistribution, actual.ResultDistribution);

			Assert.Equal(expected.QuestionStatistics.Count, actual.QuestionStatistics.Count);
			foreach (var key in expected.QuestionStatistics.Keys)
			{
				Assert.True(actual.QuestionStatistics.ContainsKey(key));
				CompareQuestionStatistics(expected.QuestionStatistics[key],
					actual.QuestionStatistics[key]);
			}

			Assert.Equal(expected.AmountOfUniqueEmails, actual.AmountOfUniqueEmails);
			Assert.Equal(expected.AttemptDistribution, actual.AttemptDistribution);
		}

        [Fact]
        public void TestOneQuiz()
        {
            var parsedData = Enumerable.Repeat(_personTestResult, 1);
            var actual = DataAnalyser.Analyze(parsedData);

			CompareDataAnalyserReports(_dataAnalyserReport, actual);
        }

		[Fact]
		public void TestAddingNewData()
		{
			var parsedData = Enumerable.Repeat(_personTestResult, 0);
			var actual = DataAnalyser.Analyze(parsedData);

			parsedData = Enumerable.Repeat(_personTestResult, 1);
			actual.AddNewData(parsedData);

			CompareDataAnalyserReports(_dataAnalyserReport, actual);
		}
    }
}
