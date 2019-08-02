using Xunit;
using QuizData.Analyser.Models;
using System.Linq;

namespace QuizData.Analyser.Test
{
    public class DataAnalyserTests
    {
        internal void CompareQuestionStatistics(QuestionStatistics expected, QuestionStatistics actual)
        {
            Assert.Equal(expected.AnswersDistribution, actual.AnswersDistribution);
            Assert.Equal(expected.RightAnswerIndex, actual.RightAnswerIndex);
            Assert.Equal(expected.RightAnswersAmount, actual.RightAnswersAmount);
            Assert.Equal(expected.WrongAnswersAmount, actual.WrongAnswersAmount);
        }

        internal void ComparePersonStatistics(PersonStatistics expected, PersonStatistics actual)
        {
            Assert.Equal(expected.AmountOfAttempts, actual.AmountOfAttempts);
            Assert.Equal(expected.Results, actual.Results);
        }

        internal void CompareDataAnalyserReports(DataAnalyserReport expected, DataAnalyserReport actual)
        {
            Assert.Equal(expected.TotalAmountOfTests, actual.TotalAmountOfTests);

            Assert.Equal(expected.PersonStatistics.Count, actual.PersonStatistics.Count);
            foreach (var key in expected.PersonStatistics.Keys)
            {
                Assert.True(actual.PersonStatistics.ContainsKey(key));
                ComparePersonStatistics(expected.PersonStatistics[key],
                    actual.PersonStatistics[key]);
            }

            Assert.Equal(expected.ResultDistribution, actual.ResultDistribution);

            Assert.Equal(expected.QuestionStatistics.Count, actual.QuestionStatistics.Count);
            foreach (var key in expected.QuestionStatistics.Keys)
            {
                Assert.True(actual.QuestionStatistics.ContainsKey(key));
                CompareQuestionStatistics(expected.QuestionStatistics[key],
                    actual.QuestionStatistics[key]);
            }

            Assert.Equal(expected.AmountOfUniqueEmails, actual.AmountOfUniqueEmails);
            Assert.Equal(expected.AttemptDistribution, actual.AttemptDistribution);
        }

        [Fact]
        public void TestOneQuiz()
        {
            var parsedData = Enumerable.Repeat(Resources.PersonTestResult, 1);
            var actual = DataAnalyser.Analyze(parsedData);

            CompareDataAnalyserReports(Resources.DataAnalyserReport, actual);
        }

        [Fact]
        public void TestAddingNewData()
        {
            var parsedData = Enumerable.Repeat(Resources.PersonTestResult, 0);
            var actual = DataAnalyser.Analyze(parsedData);

            parsedData = Enumerable.Repeat(Resources.PersonTestResult, 1);
            actual.AddNewData(parsedData);

            CompareDataAnalyserReports(Resources.DataAnalyserReport, actual);
        }
    }
}
