using NUnit.Framework;
using MeterUpload;
using MeterUpload.MeterReadingModels;
using System.Collections.Generic;
using System;
using System.Linq;
namespace UnitTests
{
    class ETLTest
    {
        ETL etl = null;
        readonly Func<string[], MeterReadingBase> createMeterReadingCust1 = 
            (string[] csvcolumns) => new Customer1MeterReading(int.Parse(csvcolumns[0]), csvcolumns[1], csvcolumns[2]);
        readonly Func<string[], MeterReadingBase> createMeterReadingCust2 =
            (string[] csvcolumns) => new Customer2MeterReading(int.Parse(csvcolumns[0]), csvcolumns[1], csvcolumns[2], csvcolumns[3]);

        [SetUp]
        public void Init()
        {
            etl = new ETL();
        }

        [TestCase("12345,2019-04-23 12:25:00,12345")]
        public void Cust1createFunctionReturnsCust1Class(string csvLine)
        {
            var result = etl.getMeterReadingsFromCSVlines(new[] { csvLine }, createMeterReadingCust1);
            Assert.IsInstanceOf<Customer1MeterReading>(result.First());
        }

        [TestCase("12345,2019-04-23 12:25:00,12345,extradata")]
        public void Cust2createFunctionReturnsCust2Class(string csvLine)
        {
            var result = etl.getMeterReadingsFromCSVlines(new[] { csvLine }, createMeterReadingCust2);
            Assert.IsInstanceOf<Customer2MeterReading>(result.First());
        }

        [TestCase("12345,2019-04-23 12:25:00")]
        public void InsufficientCSVColumnsThrowOutOfRangeExceptions(string csvLine)
        {
            void action() => etl.getMeterReadingsFromCSVlines(new[] { csvLine }, createMeterReadingCust1);
            Assert.Throws<IndexOutOfRangeException>(action);
        }
    }
}
