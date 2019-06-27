using System;
using System.Collections.Generic;
using System.Text;

namespace QuizResults.Models
{
	public class Question
	{
		public string QuestionText { get; set; }
		public List<string> AnswersList { get; set; }
		public uint CorrectAnswerIndex { get; set; }

		public override bool Equals(object obj)
		{
			var question = obj as Question;
			return question != null &&
				   QuestionText == question.QuestionText &&
				   EqualityComparer<List<string>>.Default.Equals(AnswersList, question.AnswersList) &&
				   CorrectAnswerIndex == question.CorrectAnswerIndex;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(QuestionText, AnswersList, CorrectAnswerIndex);
		}
	}
}
