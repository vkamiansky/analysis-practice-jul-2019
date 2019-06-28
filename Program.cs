using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace QuizResults
{
	class Program
	{
		static IConfigurationRoot BuildConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			return builder.Build();
		}

		static void Main(string[] args)
		{
			var parser = new CsvParser();
			var config = BuildConfiguration();
			var data = parser.ParseFile(config["DataFilePath"]);
			if (parser.ErrorMessage != null)
			{
				Console.WriteLine("Parsing failed");
				Console.WriteLine(parser.ErrorMessage);
			}
		}
	}
}
