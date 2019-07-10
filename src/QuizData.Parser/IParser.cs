using System.Collections.Generic;
using System.IO;
using System.Text;
using QuizData.Parser.Models;

namespace QuizData.Parser
{
    public interface IParser
    {
        IEnumerable<PersonTestResult> ParseFile(string path, Encoding encoding);

		IEnumerable<PersonTestResult> ParseStream(Stream stream, Encoding encoding);
    }
}