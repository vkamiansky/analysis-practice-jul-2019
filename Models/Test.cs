using System;
using System.Collections.Generic;
using System.Text;

namespace QuizResults.Models
{
	public class Test
	{
		public Person Person { get; set; }
		public List<Answer> Answers { get; set; }
		public uint Result { get; set; }
		public string Notes { get; set; }
	}
}
