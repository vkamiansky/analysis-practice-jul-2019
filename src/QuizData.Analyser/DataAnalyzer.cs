using System;
using System.Collections.Generic;
using System.Text;
using QuizData.Analyser.Models;
using QuizData.Parser.Models;

namespace QuizData.Analyser
{
	public static class DataAnalyser
	{
        private static uint _minNumberForAdvStat;

        public static void CalculateAdditionalInfo(PersonStatistics pStatistics)
        {
            if (pStatistics.AmountOfAttempts > _minNumberForAdvStat - 1)
            {
                var x = new double[pStatistics.AmountOfAttempts];
                for (var i = 0; i < x.Length; i++)
                {
                    x[i] = i + 1;
                }

                pStatistics.AdditionalInfo =
                    LinearApproximation.LinearApproximation.GetLinearApproximation(x,
                    pStatistics.Results.ConvertAll(Convert.ToDouble));
            }
            else
            {
                pStatistics.AdditionalInfo = null;
            }
        }

        /// <summary>
        /// Performs data analysis
        /// </summary>
        /// <param name="data">Data for analysis</param>
        /// <param name="minNumberForAdvStat">Minimum number of tests for one person to build advanced statistics</param>
        /// <returns>Report</returns>
		public static DataAnalyserReport Analyze(IEnumerable<PersonTestResult> data, uint minNumberForAdvStat = 4)
		{
            _minNumberForAdvStat = minNumberForAdvStat;

			var report = new DataAnalyserReport();

			// Collection of pair <Email, Amount of attempts>
			var personStatistics = new Dictionary<string, PersonStatistics>();

			// Collection of pair <Result, Amount>
			var resultDistribution = new Dictionary<uint, uint>();

			// Statistics on questions
			var qStatistics = new Dictionary<string, QuestionStatistics>();

			var totalAmount = 0U;
			var maxNumberOfAttempts = 0U;
			var personWithMaxNumberOfAttempts = "";

			foreach (var testResult in data)
			{
				if (!personStatistics.ContainsKey(testResult.Person.Email))
					personStatistics.Add(testResult.Person.Email, new PersonStatistics());
				personStatistics[testResult.Person.Email].Results.Add(testResult.Result);

				if (personStatistics[testResult.Person.Email].AmountOfAttempts > maxNumberOfAttempts)
				{
					maxNumberOfAttempts = personStatistics[testResult.Person.Email].AmountOfAttempts;
					personWithMaxNumberOfAttempts = testResult.Person.Email;
				}

				if (!resultDistribution.ContainsKey(testResult.Result))
					resultDistribution.Add(testResult.Result, 0);
				resultDistribution[testResult.Result]++;

				totalAmount++;

				// Collect statistics on questions
				foreach (var answer in testResult.Answers)
				{
					if (!qStatistics.ContainsKey(answer.Question.QuestionText))
						qStatistics.Add(answer.Question.QuestionText, new QuestionStatistics());

					if (answer.AnswerIndex == answer.Question.CorrectAnswerIndex)
						qStatistics[answer.Question.QuestionText].RightAnswersAmount++;
					else
						qStatistics[answer.Question.QuestionText].WrongAnswersAmount++;

					qStatistics[answer.Question.QuestionText].AnswersDistribution[answer.AnswerIndex]++;
					qStatistics[answer.Question.QuestionText].RightAnswerIndex = answer.Question.CorrectAnswerIndex;
				}
			}

            foreach (var pStatistics in personStatistics)
            {
                CalculateAdditionalInfo(pStatistics.Value);
            }

			report.TotalAmountOfTests = totalAmount;
			report.PersonStatistics = personStatistics;
			report.ResultDistribution = resultDistribution;
			report.QuestionStatistics = qStatistics;

			return report;
		}

		/// <summary>
		/// Analyzes new information and adds data to an existing report
		/// </summary>
		/// <param name="report">An existing report</param>
		/// <param name="newData">Collection of new tests</param>
		public static void AddNewData(this DataAnalyserReport report, IEnumerable<PersonTestResult> newData)
		{
			foreach (var el in newData)
				AddNewData(report, el);
		}

		/// <summary>
		/// Analyzes new information and adds data to an existing report
		/// </summary>
		/// <param name="report">An existing report</param>
		/// <param name="newData">Information about new test</param>
		public static void AddNewData(this DataAnalyserReport report, PersonTestResult newData)
		{
			if (!report.PersonStatistics.ContainsKey(newData.Person.Email))
				report.PersonStatistics.Add(newData.Person.Email, new PersonStatistics());
			report.PersonStatistics[newData.Person.Email].Results.Add(newData.Result);
            CalculateAdditionalInfo(report.PersonStatistics[newData.Person.Email]);

			if (!report.ResultDistribution.ContainsKey(newData.Result))
				report.ResultDistribution.Add(newData.Result, 0U);
			report.ResultDistribution[newData.Result]++;

			report.TotalAmountOfTests++;

			foreach (var answer in newData.Answers)
			{
				if (!report.QuestionStatistics.ContainsKey(answer.Question.QuestionText))
					report.QuestionStatistics.Add(answer.Question.QuestionText, new QuestionStatistics());
				var el = report.QuestionStatistics[answer.Question.QuestionText];

				if (answer.AnswerIndex == answer.Question.CorrectAnswerIndex)
					el.RightAnswersAmount++;
				else
					el.WrongAnswersAmount++;

				el.RightAnswerIndex = answer.Question.CorrectAnswerIndex;

				el.AnswersDistribution[answer.AnswerIndex]++;
			}
		}
	}
}
