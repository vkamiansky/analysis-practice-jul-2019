using System;
using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
	public class DataAnalyserReport
	{
		public uint TotalAmountOfTests { get; set; }
		public Dictionary<uint, uint> ResultDistribution { get; set; }
		public Dictionary<string, PersonStatistics> PersonStatistics { get; set; }
		public Dictionary<string, QuestionStatistics> QuestionStatistics { get; set; }

		public uint AmountOfUniqueEmails
		{
			get
			{
				return (uint)PersonStatistics.Count;
			}
		}

		public uint[] AttemptDistribution
		{
			get
			{
				var maxNumberOfAttempts = 0U;
				foreach (var el in PersonStatistics)
				{
					maxNumberOfAttempts = Math.Max(maxNumberOfAttempts, el.Value.AmountOfAttempts);
				}
				var result = new uint[maxNumberOfAttempts];
				foreach (var el in PersonStatistics)
				{
					result[el.Value.AmountOfAttempts - 1]++;
				}
				return result;
			}
		}

        public (NumericDistribution kDistribution, NumericDistribution bDistribution) GetAdditionalInfo()
        {
            var minK = double.MaxValue;
            var maxK = double.MinValue;
            var minB = double.MaxValue;
            var maxB = double.MinValue;

            var personsWithAddInfoCount = 0;

            foreach (var el in PersonStatistics)
            {
                if (el.Value.AdditionalInfo != null)
                {
                    if (el.Value.AdditionalInfo.Value.K < minK)
                        minK = el.Value.AdditionalInfo.Value.K;
                    if (el.Value.AdditionalInfo.Value.K > maxK)
                        maxK = el.Value.AdditionalInfo.Value.K;

                    if (el.Value.AdditionalInfo.Value.B < minB)
                        minB = el.Value.AdditionalInfo.Value.B;
                    if (el.Value.AdditionalInfo.Value.B > maxB)
                        maxB = el.Value.AdditionalInfo.Value.B;

                    personsWithAddInfoCount++;
                }
            }

            if (personsWithAddInfoCount == 0)
                return (null, null);

            var kDistribution = new NumericDistribution(minK, maxK, 10);
            var bDistribution = new NumericDistribution(minB, maxB, 10);
            foreach (var el in PersonStatistics)
            {
                if (el.Value.AdditionalInfo != null)
                {
                    kDistribution.AddNumeric(el.Value.AdditionalInfo.Value.K);
                    bDistribution.AddNumeric(el.Value.AdditionalInfo.Value.B);
                }
            }

            return (kDistribution, bDistribution);
        }
	}
}
