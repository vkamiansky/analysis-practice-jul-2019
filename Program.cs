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
			var parser = new Parser();
			var config = BuildConfiguration();
			var data = parser.ParseFile(config["DataFilePath"]);
		}
	}
}
