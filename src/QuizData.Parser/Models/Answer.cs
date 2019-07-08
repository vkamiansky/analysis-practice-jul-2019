using System;
using System.Collections.Generic;
using System.Text;

namespace QuizData.Parser.Models
{
	public class Answer
	{
		public Question Question { get; set; }
		public uint AnswerIndex { get; set; }
	}
}
