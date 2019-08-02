﻿using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Collections.Generic;
using QuizData.Analyser.Models.DataBlocks;
using OfficeOpenXml.Drawing.Chart;

namespace QuizData.ExcelReport
{
    public class ExcelReporter
    {
        private ExcelPackage _package;
        private ExcelWorksheetWrapper _main;
        private ExcelWorksheetWrapper _temp;
        private ExcelWorksheetWrapper _questions;

        public ExcelReporter()
        {
            _package = new ExcelPackage();
            _temp = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("_Temp"));
            _main = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("Главная"));
            _questions = new ExcelWorksheetWrapper(_package.Workbook.Worksheets.Add("Вопросы"));
        }

        public void WriteDataBlock(IDataBlock dataBlock, ExcelWorksheetWrapper to)
        {
            if (dataBlock is ScalarDataBlock scalarDataBlock)
            {
                WriteScalarDataBlock(scalarDataBlock, to);
            }
            else if (dataBlock is DistributionDataBlock<string, uint> doubleDistributionDataBlock)
            {
                WriteDistributionDataBlock(doubleDistributionDataBlock, to);
            }
            else if (dataBlock is DistributionDataBlock<uint, uint> uintDistributionDataBlock)
            {
                WriteDistributionDataBlock(uintDistributionDataBlock, to);
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, uint> c)
            {
                WriteDoubleDistributionDataBlock(c, to);
            }
            else if (dataBlock is DoubleDistributionDataBlock<uint, double?> d)
            {
                WriteDoubleDistributionDataBlock(d, to);
            }
            else
            {
                throw new System.ArgumentException("DataBlock wasn't recognized");
            }
        }

        public void WriteDataBlocks(IEnumerable<IDataBlock> dataBlocks, ExcelWorksheetWrapper to)
        {
            foreach (var dataBlock in dataBlocks)
                WriteDataBlock(dataBlock, to);
        }

        public void WriteScalarDataBlock(ScalarDataBlock dataBlock, ExcelWorksheetWrapper to)
        {
            to.Write(dataBlock.Caption);
            to.WriteLine(dataBlock.Data);
        }

        public void WriteDistributionDataBlock<TKey, TValue>(DistributionDataBlock<TKey, TValue> db, ExcelWorksheetWrapper to)
        {
            _temp.WriteLine(db.Title);

            var signatureRange = _temp.StartRangeHere();
            foreach (var pair in db.Data.First())
            {
                _temp.Write(pair.Key);
            }
            _temp.GoBack();
            _temp.FinishRangeHere(signatureRange);

            _temp.WriteLine();

            var dataRanges = new List<ExcelRange>(db.Data.Count());

            foreach (var dataRow in db.Data)
            {
                var dataRange = _temp.StartRangeHere();
                foreach (var pair in dataRow)
                {
                    _temp.Write(pair.Value);
                }
                _temp.GoBack();
                _temp.FinishRangeHere(dataRange);
                dataRanges.Add(dataRange);

                _temp.WriteLine();
            }

            if (db.Data.Length < 2)
                _temp.CreateChart(db.Title, to, 10, signatureRange, dataRanges);
            else
                _temp.CreateChart(db.Title, to, 10, signatureRange, dataRanges, eChartType.ColumnStacked);
        }

        public void WriteDoubleDistributionDataBlock<TKey, TValue>(DoubleDistributionDataBlock<TKey, TValue> db, ExcelWorksheetWrapper to)
        {
            _temp.SetPos1();

            for (var i = 0; i < db.Data.Count(); i++)
            {
                var current = db.Data.ElementAt(i);
                _temp.Write(current.Key);
                for (var j = 0; j < current.Value.Count(); j++)
                {
                    _temp.Write(current.Value.ElementAt(j));
                }
                _temp.WriteLine();
            }

            _temp.GoBack();
            _temp.SetPos2();
            _temp.WriteLine();
            var chartTitle = string.IsNullOrEmpty(db.Title) ? "" : db.Title;
            var chartName = chartTitle + System.Guid.NewGuid();
            _temp.Create3DChart(chartName, chartTitle, to,
                new[] { db.Interval1ValueTitle, db.Interval2ValueTitle, db.MeasuredValueTitle });
        }

        public void ToStream(Stream stream, IEnumerable<IDataBlock> mainData, IEnumerable<IDataBlock> questionsData)
        {
            WriteDataBlocks(mainData, _main);
            WriteDataBlocks(questionsData, _questions);

            _package.SaveAs(stream);
        }
    }
}
