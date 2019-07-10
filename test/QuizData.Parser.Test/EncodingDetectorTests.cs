using System.Text;
using Xunit;

namespace QuizData.Parser.Test
{
	public class EncodingDetectorTests
	{
		[Theory]
		[InlineData(866)]
		[InlineData(1251)]
		[InlineData(65001)]
		public void TestDetectingCodepages(int codepage)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var stream = Common.CreateStreamFromText(
				Common.TestResultsDataString,
				Encoding.GetEncoding(codepage));
			var encoding = EncodingDetector.GetEncoding(stream);
			Assert.Equal(codepage, encoding.CodePage);
		}
	}
}
