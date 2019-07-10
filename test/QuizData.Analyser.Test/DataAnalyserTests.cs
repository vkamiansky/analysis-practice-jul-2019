using Xunit;
using QuizData.Analyser.Models;
using QuizData.Parser.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuizData.Analyser.Test
{
    public class DataAnalyserTests
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
            var person = new Parser.Models.Person
            {
                Name = "Иван",
                Email = "email@site.com"
            };

            var answers = new List<Answer>()
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
            };

            var parsedData = Enumerable.Repeat(
                new PersonTestResult
                {
                    Person = person,
                    Answers = answers,
                    Result = 60,
                    Notes = "empty notes"
                }, 1);

            var results = DataAnalyser.Analyze(parsedData);

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
