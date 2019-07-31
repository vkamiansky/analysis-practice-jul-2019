using System.Collections.Generic;
using System.IO;
using System.Text;
using QuizData.Analyser.Models.DataBlocks;

namespace QuizData.TextReport
{
    public static class TextReporter
    {
        public static void WriteDataBlock(IDataBlock dataBlock, StreamWriter writer)
        {
            if (dataBlock is ScalarDataBlock scalarDataBlock)
            {
                WriteScalarDataBlock(scalarDataBlock, writer);
            }
            else if (dataBlock is DistributionDataBlock<string, uint> doubleDistributionDataBlock)
            {
                WriteDistributionDataBlock(doubleDistributionDataBlock, writer);
            }
            else if (dataBlock is DistributionDataBlock<uint, uint> uintDistributionDataBlock)
            {
                WriteDistributionDataBlock(uintDistributionDataBlock, writer);
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, uint> c)
            {
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, double?> d)
            {
            }
            else
            {
                throw new System.ArgumentException("DataBlock wasn't recognized");
            }
        }

        public static void WriteDataBlocks(IEnumerable<IDataBlock> dataBlocks, StreamWriter writer)
        {
            foreach (var dataBlock in dataBlocks)
                WriteDataBlock(dataBlock, writer);
        }

        public static void WriteScalarDataBlock(ScalarDataBlock dataBlock, StreamWriter writer)
        {
            if (!string.IsNullOrEmpty(dataBlock.Caption) && dataBlock.Data != null)
            {
                writer.Write(dataBlock.Caption);
                writer.Write(' ');
                writer.WriteLine(dataBlock.Data);
            }
            else
            {
                writer.WriteLine();
            }
        }

        public static void WriteDistributionDataBlock<TKey, TValue>(DistributionDataBlock<TKey, TValue> db, StreamWriter writer)
        {
            writer.WriteLine();
            writer.Write(db.Title);
            writer.WriteLine(":");
            foreach (var el in db.Data)
            {
                writer.Write(el.Key);
                writer.Write(": ");
                writer.WriteLine(el.Value);
            }
        }

        public static void ToStream(Stream stream, IEnumerable<IDataBlock> mainData, IEnumerable<IDataBlock> questionsData)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4 * 1024, true))
            {
                WriteDataBlocks(mainData, writer);
                writer.WriteLine();
                WriteDataBlocks(questionsData, writer);
            }
        }
    }
}
