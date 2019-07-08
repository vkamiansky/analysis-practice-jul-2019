using System;
using System.Collections.Generic;
using System.Text;
using QuizData.Analyser.Models;
using QuizData.Parser.Models;

namespace QuizData.Analyser
{
	public class DataAnalyser
	{
		public static DataAnalyserReport Analyze(IEnumerable<PersonTestResult> data)
		{
			var report = new DataAnalyserReport();

			// Collection of pair <Email, Number of attempts>
			var attemptsNumber = new Dictionary<string, int>();

			// Collection of pair <Result, Amount>
			var resultDistribution = new Dictionary<uint, uint>();

			var totalAmount = 0U;
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

			report.TotalAmountOfTests = totalAmount;
			report.AmountOfUniqueEmails = (uint)attemptsNumber.Count;

			var attemptDistribution = new uint[maxNumberOfAttempts];
			foreach (var el in attemptsNumber)
			{
				attemptDistribution[el.Value - 1]++;
			}

			report.AttemptDistribution = attemptDistribution;

			report.ResultDistribution = resultDistribution;

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

			report.QuestionStatistics = qStatistics;

			return report;
		}
	}
}
