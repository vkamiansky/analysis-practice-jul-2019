using System;
using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
	public class DataAnalyserReport
	{
		public uint TotalAmountOfTests { get; set; }
		public Dictionary<string, uint> AmountOfAttempts { get; set; }
		public Dictionary<uint, uint> ResultDistribution { get; set; }
		public Dictionary<string, QuestionStatistics> QuestionStatistics { get; set; }

		public uint AmountOfUniqueEmails
		{
			get
			{
				return (uint)AmountOfAttempts.Count;
			}
		}

		public uint[] AttemptDistribution
		{
			get
			{
				var maxNumberOfAttempts = 0U;
				foreach (var el in AmountOfAttempts)
				{
					maxNumberOfAttempts = Math.Max(maxNumberOfAttempts, el.Value);
				}
				var result = new uint[maxNumberOfAttempts];
				foreach (var el in AmountOfAttempts)
				{
					result[el.Value - 1]++;
				}
				return result;
			}
		}
	}
}
