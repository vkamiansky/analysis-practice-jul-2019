using QuizData.Analyser.Models;
using QuizData.ExcelReport;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Xunit;

namespace QuizData.Report.Test
{
    public class ExcelReporterTests
    {
        private DataAnalyserReport _dataAnalyserReport { get; } = new DataAnalyserReport
        {
            TotalAmountOfTests = 1U,
            PersonStatistics = new Dictionary<string, PersonStatistics>()
            {
                { "email@site.com", new PersonStatistics()
                    {
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

        [Fact]
        public void OneCorrectTest()
        {
            var reporter = new ExcelReporter();
            using (var stream = new MemoryStream())
            {
                reporter.ToStream(stream, _dataAnalyserReport);

                using (var md5 = MD5.Create())
                {
                    var expected = new byte[]
                    {
                        212, 29, 140, 217, 143, 0, 178,
                        4, 233, 128, 9, 152, 236, 248, 66, 126
                    };
                    var actual = md5.ComputeHash(stream);
                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
