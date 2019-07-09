using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QuizData.Parser.Models;

namespace QuizData.Parser
{
	public class CsvParser
	{
		/// <summary>
		/// Null when everything is OK
		/// </summary>
		public string ErrorMessage { get; private set; }

		/// <summary>
		/// Last read line number
		/// </summary>
		public int CurrentLineNumber { get; private set; }

		private StreamReader _reader;

		private string ReadLine()
		{
			CurrentLineNumber++;
			return _reader.ReadLine();
		}

		private bool IsEmail(string txt)
		{
			return !string.IsNullOrEmpty(txt) &&
				   txt.Contains("@") &&
				   txt.Contains(".") &&
				   !txt.Contains(" ");
		}

		private PersonTestResult ReadTest()
		{
			// Read email
			var line = ReadLine();
			var lineStart = "Email: ";
			if (!line.Contains(lineStart))
				return null;
			var email = line.Substring(line.IndexOf(lineStart) + lineStart.Length);
			if (!IsEmail(email))
			{
				ErrorMessage = string.Format("Incorrect email {0} at the line {1}", email, CurrentLineNumber);
				return null;
			}

			// Read user name
			line = ReadLine();
			lineStart = "Имя: ";
			if (!line.StartsWith(lineStart))
			{
				ErrorMessage = string.Format("Incorrect format: name expected at the line {0}", CurrentLineNumber);
				return null;
			}
			var name = line.Substring(lineStart.Length);

			// Read notes
			line = ReadLine();
			string notes = null;
			lineStart = "Заметки: ";
			if (!line.StartsWith(lineStart))
			{
				ErrorMessage = string.Format("Incorrect format: notes expected at the line {0}", CurrentLineNumber);
				return null;
			}

			// If this field is not empty
			if (line.Length > lineStart.Length)
				notes = line.Substring(lineStart.Length);

			// Notes may consist of several lines
			line = ReadLine();
			while (!line.StartsWith("Общий результат(%): "))
			{
				notes += Environment.NewLine + line;
				line = ReadLine();
			}

			// Read test result
			lineStart = "Общий результат(%): ";
			if (!line.StartsWith(lineStart))
			{
				ErrorMessage = string.Format("Incorrect format: result expected at the line {0}", CurrentLineNumber);
				return null;
			}
			var resultStr = line.Substring(lineStart.Length);
			if (!uint.TryParse(resultStr, out uint result))
			{
				ErrorMessage = string.Format("Incorrect result {0} at the line {1}", resultStr, CurrentLineNumber);
				return null;
			}

			// Skip 3 lines
			for (var i = 0; i < 3; i++)
				ReadLine();

			// Read 5 (or more) questions
			var answers = new List<Answer>(5);
			while (true)
			{
				var answer = ReadAnswer();
				if (answer == null)
					break;
				answers.Add(answer);

				// Skip empty line after question
				ReadLine();
			}

			var person = new Person
			{
				Name = name,
				Email = email
			};

			return new PersonTestResult
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
			var line = ReadLine();
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
				line = ReadLine();
				var answer = line.Substring(line.LastIndexOf(')') + 1);
				list.Add(answer);
				if (line.Contains("(+)"))
					correctAnswerIndex = i;
				if (line.Contains("(v)"))
					userAnswerIndex = i;
			}
			if (!correctAnswerIndex.HasValue)
			{
				ErrorMessage = string.Format("Question with no correct answer at the line {0}", CurrentLineNumber);
				return null;
			}
			if (!userAnswerIndex.HasValue)
			{
				ErrorMessage = string.Format("Question with no user answer at the line {0}", CurrentLineNumber);
				return null;
			}

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

		public IEnumerable<PersonTestResult> ParseFile(string path)
		{
			return ParseStream(new FileStream(path, FileMode.Open));
		}

		public IEnumerable<PersonTestResult> ParseStream(Stream stream)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			using (_reader = new StreamReader(stream, Encoding.GetEncoding(866)))
			{
				CurrentLineNumber = -1;
				ErrorMessage = null;
				ReadLine();
				while (!_reader.EndOfStream)
				{
					var test = ReadTest();
					if (test == null)
						continue;
					yield return test;
				}
			}
		}
	}
}
