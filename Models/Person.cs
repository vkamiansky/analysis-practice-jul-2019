using System;
using System.Collections.Generic;
using System.Text;

namespace QuizResults.Models
{
	public class Person
	{
		public string Name { get; set; }
		public string Email { get; set; }

		public override bool Equals(object obj)
		{
			var person = obj as Person;
			return person != null &&
				   Name == person.Name &&
				   Email == person.Email;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Email);
		}
	}
}
