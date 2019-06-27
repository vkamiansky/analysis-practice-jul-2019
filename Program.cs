namespace QuizResults
{
	class Program
	{
		static void Main(string[] args)
		{
			var parser = new Parser();
			var holyJS = parser.ParseFile("../../../holyjs.csv");
			var dotnext = parser.ParseFile("../../../dotnext.csv");
		}
	}
}
