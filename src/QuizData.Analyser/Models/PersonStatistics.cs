using System.Collections.Generic;

namespace QuizData.Analyser.Models
{
    public class PersonStatistics
    {
        public List<uint> Results { get; set; } = new List<uint>();
        public (double K, double B, double R)? AdditionalInfo { get; set; }

        public uint AmountOfAttempts
        {
            get
            {
                return (uint)Results.Count;
            }
        }
    }
}
