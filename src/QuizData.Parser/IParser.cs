using System.Collections.Generic;
using System.IO;
using QuizData.Parser.Models;

namespace QuizData.Parser
{
    public interface IParser
    {
        IEnumerable<PersonTestResult> ParseFile(string path);

		IEnumerable<PersonTestResult> ParseStream(Stream stream);
    }
}