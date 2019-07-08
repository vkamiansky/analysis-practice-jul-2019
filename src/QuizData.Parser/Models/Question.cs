using System;
using System.Collections.Generic;
using System.Text;

namespace QuizData.Parser.Models
{
	public class Question
	{
		public string QuestionText { get; set; }
		public List<string> AnswersList { get; set; }
		public uint CorrectAnswerIndex { get; set; }
	}
}
