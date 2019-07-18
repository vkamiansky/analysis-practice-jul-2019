using Microsoft.Extensions.Configuration;
using QuizData.Analyser;
using QuizData.Parser;
using QuizData.TextReport;
using System;
using System.IO;
using System.Text;

namespace QuizData
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
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var encoding = EncodingDetector.GetEncoding(config["DataFilePath"]);
			var data = parser.ParseFile(config["DataFilePath"], encoding);
			if (parser.ErrorMessage != null)
			{
				Console.WriteLine("Parsing failed");
				Console.WriteLine(parser.ErrorMessage);
			}

			var report = DataAnalyser.Analyze(data);
            using (var stream = new FileStream("report.txt", FileMode.Create))
            {
                TextReporter.ToStream(stream, report);
            }
        }
	}
}
