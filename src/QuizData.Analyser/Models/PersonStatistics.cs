using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
	public class PersonStatistics
	{
		public uint AmountOfAttempts { get; set; }
		public List<uint> Results { get; set; } = new List<uint>();
	}
}
