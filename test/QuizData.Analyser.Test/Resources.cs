using QuizData.Analyser.Models;
using QuizData.Analyser.Models.DataBlocks;
using QuizData.Parser.Models;
using System.Collections.Generic;

namespace QuizData.Analyser.Test
{
    public static class Resources
    {
        public static PersonTestResult PersonTestResult = new PersonTestResult()
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

        public static DataAnalyserReport DataAnalyserReport = new DataAnalyserReport
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

        public static List<IDataBlock> MainData = new List<IDataBlock>()
        {
            new ScalarDataBlock(1U, "Всего тестов:"),
            new ScalarDataBlock(1U, "Количество уникальных e-mail'ов:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 1)
                    }
                },
                "Распределение попыток"
            ),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(60, 1)
                    }
                },
                "Распределение результатов"
            )
        };

        public static List<IDataBlock> QuestionsData = new List<IDataBlock>()
        {
            // 1st question
            new ScalarDataBlock("Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?", "Вопрос:"),
            new ScalarDataBlock(1U, "Правильных ответов:"),
            new ScalarDataBlock(0U, "Неправильных ответов:"),
            new ScalarDataBlock(1U, "Всего ответов:"),
            new ScalarDataBlock(3U, "Правильный ответ:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 0),
                        new KeyValuePair<uint, uint>(2, 0),
                        new KeyValuePair<uint, uint>(3, 1),
                        new KeyValuePair<uint, uint>(4, 0),
                    }
                },
                "Ответы пользователей"
            ),
            new ScalarDataBlock("", ""),

            // 2nd question
            new ScalarDataBlock("Багет - это хлеб, а хлеб ? это багет?", "Вопрос:"),
            new ScalarDataBlock(0U, "Правильных ответов:"),
            new ScalarDataBlock(1U, "Неправильных ответов:"),
            new ScalarDataBlock(1U, "Всего ответов:"),
            new ScalarDataBlock(1U, "Правильный ответ:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 0),
                        new KeyValuePair<uint, uint>(2, 0),
                        new KeyValuePair<uint, uint>(3, 0),
                        new KeyValuePair<uint, uint>(4, 1),
                    }
                },
                "Ответы пользователей"
            ),
            new ScalarDataBlock("", ""),

            // 3rd question
            new ScalarDataBlock("Путник знает, что и как заказать в таверне.", "Вопрос:"),
            new ScalarDataBlock(1U, "Правильных ответов:"),
            new ScalarDataBlock(0U, "Неправильных ответов:"),
            new ScalarDataBlock(1U, "Всего ответов:"),
            new ScalarDataBlock(1U, "Правильный ответ:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 1),
                        new KeyValuePair<uint, uint>(2, 0),
                        new KeyValuePair<uint, uint>(3, 0),
                        new KeyValuePair<uint, uint>(4, 0),
                    }
                },
                "Ответы пользователей"
            ),
            new ScalarDataBlock("", ""),

            // 4th question
            new ScalarDataBlock("Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?", "Вопрос:"),
            new ScalarDataBlock(1U, "Правильных ответов:"),
            new ScalarDataBlock(0U, "Неправильных ответов:"),
            new ScalarDataBlock(1U, "Всего ответов:"),
            new ScalarDataBlock(3U, "Правильный ответ:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 0),
                        new KeyValuePair<uint, uint>(2, 0),
                        new KeyValuePair<uint, uint>(3, 1),
                        new KeyValuePair<uint, uint>(4, 0),
                    }
                },
                "Ответы пользователей"
            ),
            new ScalarDataBlock("", ""),

            // 5th question
            new ScalarDataBlock("Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?", "Вопрос:"),
            new ScalarDataBlock(0U, "Правильных ответов:"),
            new ScalarDataBlock(1U, "Неправильных ответов:"),
            new ScalarDataBlock(1U, "Всего ответов:"),
            new ScalarDataBlock(3U, "Правильный ответ:"),
            new DistributionDataBlock<uint, uint>(
                new[]
                {
                    new List<KeyValuePair<uint, uint>>()
                    {
                        new KeyValuePair<uint, uint>(1, 1),
                        new KeyValuePair<uint, uint>(2, 0),
                        new KeyValuePair<uint, uint>(3, 0),
                        new KeyValuePair<uint, uint>(4, 0),
                    }
                },
                "Ответы пользователей"
            ),
            new ScalarDataBlock("", "")
        };
    }
}
