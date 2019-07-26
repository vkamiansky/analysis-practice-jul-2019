using System.IO;
using System.Text;

namespace QuizData.Parser
{
    public class EncodingDetector
    {
        private static void SkipFirstLine(Stream stream)
        {
            var previous = -1;
            var current = -1;
            while (true)
            {
                previous = current;
                current = stream.ReadByte();

                if (current == 10 && previous == 13)
                    break;
            }
        }

        private static bool IsItUnicode(Stream stream)
        {
            var count = 0U;
            var totalCount = 0U;

            SkipFirstLine(stream);

            for (totalCount = 0; totalCount < 500; totalCount++)
            {
                var temp = stream.ReadByte();

                if (temp == -1)
                    break;

                if (temp == 208 || temp == 209)
                {
                    count++;
                }
            }

            return (double)count / totalCount > 0.2;
        }

        private static bool IsItWindows1251(Stream stream)
        {
            var count = 0U;
            var totalCount = 0U;

            SkipFirstLine(stream);

            for (totalCount = 0; totalCount < 500; totalCount++)
            {
                var temp = stream.ReadByte();

                if (temp == -1)
                    break;

                if (temp >= 192 && temp <= 255)
                {
                    count++;
                }
            }

            return (double)count / totalCount > 0.2;
        }

        private static bool IsItOEM866(Stream stream)
        {
            var count = 0U;
            var totalCount = 0U;

            SkipFirstLine(stream);

            for (totalCount = 0; totalCount < 500; totalCount++)
            {
                var temp = stream.ReadByte();

                if (temp == -1)
                    break;

                if (temp >= 128 && temp <= 175)
                {
                    count++;
                }
                if (temp >= 224 && temp <= 239)
                {
                    count++;
                }
            }

            return (double)count / totalCount > 0.1;
        }

        private static bool IsItOEM866(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return IsItOEM866(stream);
            }
        }

        private static bool IsItWindows1251(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return IsItWindows1251(stream);
            }
        }

        private static bool IsItUnicode(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return IsItUnicode(stream);
            }
        }

        public static Encoding GetEncoding(Stream stream)
        {
            if (IsItUnicode(stream))
                return Encoding.UTF8;
            if (IsItWindows1251(stream))
                return Encoding.GetEncoding(1251);
            if (IsItOEM866(stream))
                return Encoding.GetEncoding(866);

            return null;
        }

        public static Encoding GetEncoding(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return GetEncoding(stream);
            }
        }
    }
}
