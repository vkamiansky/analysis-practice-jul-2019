using System;
using Xunit;
using QuizData.Parser.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QuizData.Parser.Tests
{
    public class CsvParserTests
    {
        private const string testResultsDataString =
@"""Subject"",""Body"",""From: (Name)"",""From: (Address)"",""From: (Type)"",""To: (Name)"",""To: (Address)"",""To: (Type)"",""CC: (Name)"",""CC: (Address)"",""CC: (Type)"",""BCC: (Name)"",""BCC: (Address)"",""BCC: (Type)"",""Billing Information"",""Categories"",""Importance"",""Mileage"",""Sensitivity""
""Результаты Иван"",""Email: email@site.com
Имя: Иван
Заметки: 
Общий результат(%): 60

Ответы:

Вопрос: Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?
(-)bread bread burger cheese
(-)bread burger cheese bread
(+)(v)bread burger bread cheese
(-)burger cheese bread bread

Вопрос: Багет - это хлеб, а хлеб ? это багет?
(+)undefined true
(-)false true
(-)true true
(-)(v)true undefined

Вопрос: Путник знает, что и как заказать в таверне.
(+)(v)[ 'bacon and eggs' ]
(-)[ 'bacon and eggs', 'coffee' ]
(-)[ 'coffee', 'bacon and eggs' ]
(-)SyntaxError: Unexpected token

Вопрос: Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?
(-)'1-23-4'
(-)-2
(+)(v)'1-2-1'
(-)'-13-4'

Вопрос: Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?
(-)(v)focus pocus
(-)focus
(+)ничего
(-)undefined
";
        internal void CompareAnswers(Answer expected, Answer actual)
        {
            Assert.Equal(expected.Question.QuestionText, actual.Question.QuestionText);
            Assert.Equal(expected.Question.AnswersList.Count, actual.Question.AnswersList.Count);
            for (var i = 0; i < actual.Question.AnswersList.Count; i++)
            {
                Assert.Equal(expected.Question.AnswersList.ElementAt(i), actual.Question.AnswersList.ElementAt(i));
            }
            Assert.Equal(expected.AnswerIndex, actual.AnswerIndex);
            Assert.Equal(expected.Question.CorrectAnswerIndex, actual.Question.CorrectAnswerIndex);
        }

        [Theory]
        [InlineData(866)]
        [InlineData(1251)]
		[InlineData(65001)]
        public void OneCorrectTest(int codePage)
        {
            var parser = new CsvParser();
            var bytes = new byte[] { };
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.GetEncoding(codePage)))
                {
                    writer.Write(testResultsDataString);
                }
                bytes = stream.ToArray();
            }

			Encoding encoding;
			using (var stream = new MemoryStream(bytes))
			{
				encoding = EncodingDetector.GetEncoding(stream);
			}

			using (var stream = new MemoryStream(bytes))
            {
                var data = parser.ParseStream(stream, encoding);
                Assert.Single(data);
                var test = data.First();

				Assert.Equal("email@site.com", test.Person.Email);
				Assert.Equal("Иван", test.Person.Name);
				Assert.Null(test.Notes);
				Assert.Equal((uint)60, test.Result);

				Assert.Equal(5, test.Answers.Count);

				var expected = new Answer
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
				};
				CompareAnswers(expected, test.Answers.ElementAt(0));

				expected = new Answer
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
				};
				CompareAnswers(expected, test.Answers.ElementAt(1));

				expected = new Answer
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
				};
				CompareAnswers(expected, test.Answers.ElementAt(2));

				expected = new Answer
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
				};
				CompareAnswers(expected, test.Answers.ElementAt(3));

				expected = new Answer
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
				};
				CompareAnswers(expected, test.Answers.ElementAt(4));

			}
        }
    }
}
