using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace QuizResults
{
	class Program
	{
		static IConfigurationRoot BuildConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			return builder.Build();
		}

		static void Main(string[] args)
		{
			var parser = new CsvParser();
			var config = BuildConfiguration();
			var data = parser.ParseFile(config["DataFilePath"]);
			if (parser.ErrorMessage != null)
			{
				Console.WriteLine("Parsing failed");
				Console.WriteLine(parser.ErrorMessage);
			}

			// Collection of pair <Email, Number of attempts>
			var attemptsNumber = new Dictionary<string, int>();

			// Collection of pair <Result, Amount>
			var resultDistribution = new Dictionary<uint, uint>();

			var totalAmount = 0;
			var maxNumberOfAttempts = 0;
			var personWithMaxNumberOfAttempts = "";

			foreach (var testResult in data)
			{
				if (!attemptsNumber.ContainsKey(testResult.Person.Email))
					attemptsNumber.Add(testResult.Person.Email, 0);
				attemptsNumber[testResult.Person.Email]++;

				if (attemptsNumber[testResult.Person.Email] > maxNumberOfAttempts)
				{
					maxNumberOfAttempts = attemptsNumber[testResult.Person.Email];
					personWithMaxNumberOfAttempts = testResult.Person.Email;
				}

				if (!resultDistribution.ContainsKey(testResult.Result))
					resultDistribution.Add(testResult.Result, 0);
				resultDistribution[testResult.Result]++;

				totalAmount++;
			}

			Console.WriteLine("Всего тестов: {0}", totalAmount);
			Console.WriteLine("Количество уникальных e-mail'ов: {0}", attemptsNumber.Count);

			var attemptDistribution = new uint[maxNumberOfAttempts];
			foreach (var el in attemptsNumber)
			{
				attemptDistribution[el.Value - 1]++;
			}

			Console.WriteLine("Распределение попыток:");
			for (var i = 0; i < attemptDistribution.Length; i++)
			{
				if (attemptDistribution[i] != 0)
					Console.WriteLine("{0}: {1}", i + 1, attemptDistribution[i]);
			}

			Console.WriteLine("Распределение результатов:");
			foreach (var el in resultDistribution)
			{
				Console.WriteLine("{0}: {1}", el.Key, el.Value);
			}

			Console.WriteLine("Участник {0} сделал {1} попыток", personWithMaxNumberOfAttempts, maxNumberOfAttempts);


			// Statistics on questions
			var qStatistics = new Dictionary<string, QuestionStatistics>();

			foreach (var testResult in data)
			{
				foreach (var answer in testResult.Answers)
				{
					if (!qStatistics.ContainsKey(answer.Question.QuestionText))
						qStatistics.Add(answer.Question.QuestionText, new QuestionStatistics());

					if (answer.AnswerIndex == answer.Question.CorrectAnswerIndex)
						qStatistics[answer.Question.QuestionText].RightAnswersAmount++;
					else
						qStatistics[answer.Question.QuestionText].WrongAnswersAmount++;
				}
			}

			foreach (var el in qStatistics)
			{
				Console.WriteLine("Вопрос: {0}\nПравильных ответов: {1}, неправильных: {2}, всего: {3}",
					el.Key, el.Value.RightAnswersAmount, el.Value.WrongAnswersAmount,
					el.Value.RightAnswersAmount + el.Value.WrongAnswersAmount);
			}
		}
	}
}
