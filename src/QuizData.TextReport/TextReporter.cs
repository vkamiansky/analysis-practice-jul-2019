using System.IO;
using System.Text;
using QuizData.Analyser.Models;

namespace QuizData.TextReport
{
	public class TextReporter
	{
		public static void ToStream(Stream stream, DataAnalyserReport report, int bufferSize = 4 * 1024)
		{
			using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize, true))
			{
				writer.WriteLine("Всего тестов: {0}", report.TotalAmountOfTests);
				writer.WriteLine("Количество уникальных e-mail'ов: {0}\n", report.AmountOfUniqueEmails);

				writer.WriteLine("Распределение попыток:");
				for (var i = 0; i < report.AttemptDistribution.Length; i++)
				{
					if (report.AttemptDistribution[i] != 0)
						writer.WriteLine("{0}: {1}", i + 1, report.AttemptDistribution[i]);
				}
				writer.WriteLine();

				writer.WriteLine("Распределение результатов:");
				foreach (var el in report.ResultDistribution)
				{
					writer.WriteLine("{0}: {1}", el.Key, el.Value);
				}
				writer.WriteLine();

				writer.WriteLine("Статистика вопросов:");
				foreach (var el in report.QuestionStatistics)
				{
					writer.WriteLine("Вопрос: {0}\nПравильных ответов: {1}, неправильных: {2}, всего: {3}\n" +
						"Правильный ответ: {4}, а люди отвечали 0:{5} 1:{6} 2:{7} 3:{8}\n",
						el.Key, el.Value.RightAnswersAmount, el.Value.WrongAnswersAmount,
						el.Value.RightAnswersAmount + el.Value.WrongAnswersAmount,
						el.Value.RightAnswerIndex, el.Value.AnswersDistribution[0], el.Value.AnswersDistribution[1],
						el.Value.AnswersDistribution[2], el.Value.AnswersDistribution[3]);
				}

                (var kDistribution, var bDistribution) = report.GetAdditionalInfo();
                if (kDistribution != null && bDistribution != null)
                {
                    writer.WriteLine("Распределение коэффициента K:");
                    foreach (var el in kDistribution.Parts)
                    {
                        writer.WriteLine("В интервале от {0:F2} и до {1:F2}: {2}",
                            el.LeftBorder, el.RightBorder, el.NumericsAmount);
                    }
                    writer.WriteLine();
                    writer.WriteLine("Распределение коэффициента B:");
                    foreach (var el in bDistribution.Parts)
                    {
                        writer.WriteLine("В интервале от {0:F2} и до {1:F2}: {2}",
                            el.LeftBorder, el.RightBorder, el.NumericsAmount);
                    }
                    writer.WriteLine();
                }
            }
		}
	}
}
