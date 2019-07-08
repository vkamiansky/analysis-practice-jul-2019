using System;
using System.Collections.Generic;
using System.Text;

namespace QuizData.Parser.Models
{
	public class PersonTestResult
	{
		public Person Person { get; set; }
		public List<Answer> Answers { get; set; }
		public uint Result { get; set; }
		public string Notes { get; set; }
	}
}
