using System;
using System.Collections.Generic;
using System.Text;

namespace QuizResults.Models
{
	public class Answer
	{
		public Question Question { get; set; }
		public uint AnswerIndex { get; set; }
	}
}
