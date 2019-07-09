using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace QuizData.Parser.Test
{
	public class EncodingDetectorTests
	{
		private const string testResultsDataString =
@"""Subject"",""Body"",""From: (Name)"",""From: (Address)"",""From: (Type)"",""To: (Name)"",""To: (Address)"",""To: (Type)"",""CC: (Name)"",""CC: (Address)"",""CC: (Type)"",""BCC: (Name)"",""BCC: (Address)"",""BCC: (Type)"",""Billing Information"",""Categories"",""Importance"",""Mileage"",""Sensitivity""
""Результаты Иван"",""Email: email@site.com
Имя: Иван
Заметки: 
Общий результат(%): 60

Ответы:

Вопрос: Мейстеры Цитадели изобрели бургеры и спешат поделиться своим открытием со всеми. Но так ли хорош рецепт?
(-)bread bread burger cheese
(-)bread burger cheese bread
(+)(v)bread burger bread cheese
(-)burger cheese bread bread

Вопрос: Багет - это хлеб, а хлеб ? это багет?
(+)undefined true
(-)false true
(-)true true
(-)(v)true undefined

Вопрос: Путник знает, что и как заказать в таверне.
(+)(v)[ 'bacon and eggs' ]
(-)[ 'bacon and eggs', 'coffee' ]
(-)[ 'coffee', 'bacon and eggs' ]
(-)SyntaxError: Unexpected token

Вопрос: Тормунд и Джон решили покушать бутербродов. Джон маленький и хочет скушать 2 бутерброда, а Тормунд большой и хочет 4. Но у Джона только 1 кусочек хлеба, а у Тормунда - только 3. Сколько кусочков хлеба им не хватает?
(-)'1-23-4'
(-)-2
(+)(v)'1-2-1'
(-)'-13-4'

Вопрос: Мелисандра хорошо управляется с магией. Получится ли у нас? Что будет выведено в консоль после расфокусировки input элемента?
(-)(v)focus pocus
(-)focus
(+)ничего
(-)undefined
";

		public Stream CreateStreamFromText(string text, Encoding encoding)
		{
			var bytes = new byte[] { };
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			using (var stream = new MemoryStream())
			{
				using (var writer = new StreamWriter(stream, encoding))
				{
					writer.Write(text);
				}
				bytes = stream.ToArray();
			}

			return new MemoryStream(bytes);
		}

		[Theory]
		[InlineData(866)]
		[InlineData(1251)]
		[InlineData(65001)]
		public void TestDetectingCodepages(int codepage)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var stream = CreateStreamFromText(testResultsDataString, Encoding.GetEncoding(codepage));
			var encoding = EncodingDetector.GetEncoding(stream);
			Assert.Equal(codepage, encoding.CodePage);
		}
	}
}
