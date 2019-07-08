using System;
using System.Collections.Generic;
using System.Text;

namespace QuizResults
{
	public class DataAnalyzerReport
	{
		public uint TotalAmountOfTests { get; set; }
		public uint AmountOfUniqueEmails { get; set; }
		public uint[] AttemptDistribution { get; set; }
		public Dictionary<uint, uint> ResultDistribution { get; set; }
		public Dictionary<string, QuestionStatistics> QuestionStatistics { get; set; }
	}
}
