using QuizData.Analyser.Models;
using QuizData.TextReport;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace QuizData.Report.Test
{
    public class TextReporterTests
    {
		private DataAnalyserReport _dataAnalyserReport { get; } = new DataAnalyserReport
		{
			TotalAmountOfTests = 1U,
			AmountOfAttempts = new Dictionary<string, uint>()
				{
					{ "email@site.com", 1 }
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

		private string _textReporterOutput =
@"Всего тестов: 1
Количество уникальных e-mail'ов: 1

Распределение попыток:
1: 1

Распределение результатов:
60: 1

Статистика вопросов:
Вопрос: Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?
Правильных ответов: 1, неправильных: 0, всего: 1
Правильный ответ: 2, а люди отвечали 0:0 1:0 2:1 3:0

Вопрос: Багет - это хлеб, а хлеб ? это багет?
Правильных ответов: 0, неправильных: 1, всего: 1
Правильный ответ: 0, а люди отвечали 0:0 1:0 2:0 3:1

Вопрос: Путник знает, что и как заказать в таверне.
Правильных ответов: 1, неправильных: 0, всего: 1
Правильный ответ: 0, а люди отвечали 0:1 1:0 2:0 3:0

Вопрос: Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?
Правильных ответов: 1, неправильных: 0, всего: 1
Правильный ответ: 2, а люди отвечали 0:0 1:0 2:1 3:0

Вопрос: Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?
Правильных ответов: 0, неправильных: 1, всего: 1
Правильный ответ: 2, а люди отвечали 0:1 1:0 2:0 3:0

";

		[Fact]
        public void TextReporterTest()
        {
			var stream = new MemoryStream();
			TextReporter.ToStream(stream, _dataAnalyserReport);

			using (var readableStream = new MemoryStream(stream.ToArray()))
			{
				using (var reader = new StreamReader(readableStream))
				{
					var text = _textReporterOutput.Split(Environment.NewLine);
					var i = 0;
					while (!reader.EndOfStream)
					{
						Assert.Equal(text[i++], reader.ReadLine());
					}
				}
			}
        }
    }
}
