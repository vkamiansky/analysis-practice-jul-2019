using System;
using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
	public class DataAnalyserReport
	{
		public uint TotalAmountOfTests { get; set; }
		public Dictionary<uint, uint> ResultDistribution { get; set; }
		public Dictionary<string, PersonStatistics> PersonStatistics { get; set; }
		public Dictionary<string, QuestionStatistics> QuestionStatistics { get; set; }

		public uint AmountOfUniqueEmails
		{
			get
			{
				return (uint)PersonStatistics.Count;
			}
		}

		public uint[] AttemptDistribution
		{
			get
			{
				var maxNumberOfAttempts = 0U;
				foreach (var el in PersonStatistics)
				{
					maxNumberOfAttempts = Math.Max(maxNumberOfAttempts, el.Value.AmountOfAttempts);
				}
				var result = new uint[maxNumberOfAttempts];
				foreach (var el in PersonStatistics)
				{
					result[el.Value.AmountOfAttempts - 1]++;
				}
				return result;
			}
		}
	}
}
