using System;
using System.IO;
using OfficeOpenXml;
using QuizData.Analyser.Models;

namespace QuizData.ExcelReporter
{
	public class ExcelReporter
	{
		public static void ToFile(string path, DataAnalyserReport report)
		{
			var package = new ExcelPackage();
			var ws1 = package.Workbook.Worksheets.Add("Worksheet1");
			ws1.Cells[1, 1].Value = "Всего тестов: ";
			ws1.Cells[1, 2].Value = report.TotalAmountOfTests;
			

			ws1.Cells[3, 1].Value = "Распределение попыток: ";
			var currentLineNumber = 4;
			for (var i = 0; i < report.AttemptDistribution.Length; i++)
			{
				if (report.AttemptDistribution[i] != 0)
				{
					ws1.Cells[currentLineNumber, 1].Value = i + 1;
					ws1.Cells[currentLineNumber, 2].Value = report.AttemptDistribution[i];
					currentLineNumber++;
				}
			}

			var max = 0U;
			var userWithMax = "";
			foreach (var pStatistics in report.PersonStatistics)
			{
				if (pStatistics.Value.AmountOfAttempts > max)
				{
					max = pStatistics.Value.AmountOfAttempts;
					userWithMax = pStatistics.Key;
				}
			}

			var chart = ws1.Drawings.AddChart("chart1", OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
			chart.Series.Add(ExcelRange.GetAddress(4, 2, 17, 2), ExcelRange.GetAddress(4, 1, 17, 1));

			ws1.Cells[20, 1].Value = string.Format("Результаты пользователя {0}", userWithMax);
			currentLineNumber = 21;
			for (var i = 0; i < report.PersonStatistics[userWithMax].Results.Count; i++)
			{
				ws1.Cells[currentLineNumber, 1].Value = i + 1;
				ws1.Cells[currentLineNumber, 2].Value = report.PersonStatistics[userWithMax].Results[i];
				currentLineNumber++;
			}

			var chart2 = ws1.Drawings.AddChart("chart2", OfficeOpenXml.Drawing.Chart.eChartType.Line);
			var chartSerie = chart2.Series.Add(ExcelRange.GetAddress(21, 2, 62, 2), ExcelRange.GetAddress(21, 1, 62, 1));
			var trend = chartSerie.TrendLines.Add(OfficeOpenXml.Drawing.Chart.eTrendLine.Linear);
			trend.DisplayRSquaredValue = false;

			package.SaveAs(new FileInfo("out556.xlsx"));
		}
	}
}
