using QuizData.Analyser.Models.DataBlocks;
using QuizData.TextReport;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace QuizData.Report.Test
{
    public class TextReporterTests
    {
        private List<IDataBlock> _mainData = new List<IDataBlock>()
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

        private List<IDataBlock> _questionsData = new List<IDataBlock>()
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

        private string _textReporterOutput =
@"Всего тестов: 1
Количество уникальных e-mail'ов: 1

Распределение попыток:
1 
1 

Распределение результатов:
60 
1 

Вопрос: Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?
Правильных ответов: 1
Неправильных ответов: 0
Всего ответов: 1
Правильный ответ: 3

Ответы пользователей:
1 2 3 4 
0 0 1 0 

Вопрос: Багет - это хлеб, а хлеб ? это багет?
Правильных ответов: 0
Неправильных ответов: 1
Всего ответов: 1
Правильный ответ: 1

Ответы пользователей:
1 2 3 4 
0 0 0 1 

Вопрос: Путник знает, что и как заказать в таверне.
Правильных ответов: 1
Неправильных ответов: 0
Всего ответов: 1
Правильный ответ: 1

Ответы пользователей:
1 2 3 4 
1 0 0 0 

Вопрос: Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?
Правильных ответов: 1
Неправильных ответов: 0
Всего ответов: 1
Правильный ответ: 3

Ответы пользователей:
1 2 3 4 
0 0 1 0 

Вопрос: Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?
Правильных ответов: 0
Неправильных ответов: 1
Всего ответов: 1
Правильный ответ: 3

Ответы пользователей:
1 2 3 4 
1 0 0 0 

";

        [Fact]
        public void TextReporterTest()
        {
            using (var stream = new MemoryStream())
            {
                TextReporter.ToStream(stream, _mainData, _questionsData);

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
}
