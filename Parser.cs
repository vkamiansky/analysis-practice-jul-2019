using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QuizResults.Models;

namespace QuizResults
{
	public class Parser
	{
		private StreamReader _reader;

		private bool IsEmail(string txt)
		{
			return !string.IsNullOrEmpty(txt) &&
				   txt.Contains("@") &&
				   txt.Contains(".") &&
				   !txt.Contains(" ");
		}

		private Test ReadTest()
		{
			// Read email
			var line = _reader.ReadLine();
			var lineStart = "Email: ";
			if (!line.Contains(lineStart))
				return null;
			var email = line.Substring(line.IndexOf(lineStart) + lineStart.Length);
			if (!IsEmail(email))
				throw new FormatException("Incorrect email: " + email);

			// Read user name
			line = _reader.ReadLine();
			lineStart = "Имя: ";
			if (!line.StartsWith(lineStart))
				throw new FormatException("Incorrect format: name expected");
			var name = line.Substring(lineStart.Length);

			// Read notes
			line = _reader.ReadLine();
			string notes = null;
			lineStart = "Заметки: ";
			if (!line.StartsWith(lineStart))
				throw new FormatException("Incorrect format: notes expected");
			// If this field is not empty
			if (line.Length > lineStart.Length)
				notes = line.Substring(lineStart.Length);

			// Notes may consist of several lines
			line = _reader.ReadLine();
			while (!line.StartsWith("Общий результат(%): "))
			{
				notes += Environment.NewLine + line;
				line = _reader.ReadLine();
			}

			// Read test result
			lineStart = "Общий результат(%): ";
			if (!line.StartsWith(lineStart))
				throw new FormatException("Incorrect format: result expected");
			var resultStr = line.Substring(lineStart.Length);
			if (!uint.TryParse(resultStr, out uint result))
				throw new FormatException("Incorrect result: " + resultStr);

			// Skip 3 lines
			for (var i = 0; i < 3; i++)
				_reader.ReadLine();

			// Read 5 (or more) questions
			var answers = new List<Answer>(5);
			while (true)
			{
				var answer = ReadAnswer();
				if (answer == null)
					break;
				answers.Add(answer);

				// Skip empty line after question
				_reader.ReadLine();
			}

			var person = new Person
			{
				Name = name,
				Email = email
			};

			return new Test
			{
				Person = person,
				Answers = answers,
				Result = result,
				Notes = notes
			};
		}

		private Answer ReadAnswer()
		{
			// Get question
			var line = _reader.ReadLine();
			if (line == null)
				return null;
			var lineStart = "Вопрос: ";
			if (!line.StartsWith(lineStart))
				return null;
			var questionText = line.Substring(lineStart.Length);

			// Get answers and other
			uint? correctAnswerIndex = null;
			uint? userAnswerIndex = null;
			var list = new List<string>(4);
			for (uint i = 0; i < 4; i++)
			{
				line = _reader.ReadLine();
				var answer = line.Substring(line.LastIndexOf(')') + 1);
				list.Add(answer);
				if (line.Contains("(+)"))
					correctAnswerIndex = i;
				if (line.Contains("(v)"))
					userAnswerIndex = i;
			}
			if (!correctAnswerIndex.HasValue)
				throw new FormatException("Question with no correct answer");
			if (!userAnswerIndex.HasValue)
				throw new FormatException("Qustion with no user answer");

			var question = new Question
			{
				QuestionText = questionText,
				AnswersList = list,
				CorrectAnswerIndex = correctAnswerIndex.Value
			};

			return new Answer
			{
				Question = question,
				AnswerIndex = userAnswerIndex.Value
			};
		}

		public List<Test> ParseFile(string path)
		{
			var result = new List<Test>();
			using (_reader = new StreamReader(path))
			{
				_reader.ReadLine();
				while (!_reader.EndOfStream)
				{
					var test = ReadTest();
					if (test == null)
						break;
					result.Add(test);
				}
			}
			return result;
		}
	}
}
