using NUnit.Framework;
using MeterUpload;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UnitTests
{
    public class ValidatorTests
    {
        List<int> validAccounts = null;
        List<MeterReadingBase> currentReadings = null;

        [SetUp]
        public void Init()
        {
            validAccounts = new() { 11111,22222,33333,44444 };
            currentReadings = new()
            {
                new Customer1MeterReading(11111, "2019-04-23 12:25:00", "12222"),
                new Customer1MeterReading(11111, "2019-05-23 12:25:00", "12345"),
                new Customer1MeterReading(33333, "2019-06-11 12:22:00", "00321")
            };
        }

        [TestCase(12345,false)]
        [TestCase(11111, true)]
        [TestCase(33333, true)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(99999999, false)]
        public void AccountIDValidationTest(int accID, bool expected)
        {
            List<MeterReadingBase> data = new()
            {
                new Customer1MeterReading(accID, DateTime.Now.ToString(), "")
            };

            var validator = new MeterReadingValidator(data, validAccounts, currentReadings);
            validator.ValidateAccountID();

            bool result = data.First().IsValid;
            Assert.AreEqual(result, expected);
        }

        [TestCase("12345", true)]
        [TestCase("", false)]
        [TestCase("1234", false)]
        [TestCase("123456", false)]
        [TestCase("asdf", false)]
        [TestCase("1234a", false)]
        public void ReadingValueValidationTest(string reading, bool expected)
        {
            List<MeterReadingBase> data = new()
            {
                new Customer1MeterReading(0, DateTime.Now.ToString(), reading)
            };

            var validator = new MeterReadingValidator(data, validAccounts, currentReadings);
            validator.ValidateMeterReadingValue();

            bool result = data.First().IsValid;
            Assert.AreEqual(result, expected);
        }

        [TestCase(11111, "2019-04-23 12:25:00", "12222", false)]
        [TestCase(33333, "2019-06-11 12:22:00", "00321", false)]
        [TestCase(11111, "2019-04-23 12:25:00", "12223", true)]
        public void AlreadyInDBValidationTest(int accID, string date, string reading, bool expected)
        {
            List<MeterReadingBase> data = new()
            {
                new Customer1MeterReading(accID, date, reading)
            };

            var validator = new MeterReadingValidator(data, validAccounts, currentReadings);
            validator.ValidateIfReadingAlreadyInDB();

            bool result = data.First().IsValid;
            Assert.AreEqual(result, expected);
        }

        [TestCase(11111, "2019-05-24 12:25:00", "12223", true)]
        [TestCase(11111, "2019-04-23 12:25:00", "12455", false)]
        [TestCase(33333, "2019-06-11 12:22:00", "00321", false)]
        public void ReadingIsNewerThanLatestInDBValidationTest(int accID, string date, string reading, bool expected)
        {
            List<MeterReadingBase> data = new()
            {
                new Customer1MeterReading(accID, date, reading)
            };

            var validator = new MeterReadingValidator(data, validAccounts, currentReadings);
            validator.ValidateIfReadingIsNewerThanLatestInDB();

            bool result = data.First().IsValid;
            Assert.AreEqual(result, expected);
        }
    }
}
