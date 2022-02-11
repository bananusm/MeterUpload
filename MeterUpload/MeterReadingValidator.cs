using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        private IEnumerable<MeterReadingBase> MeterReadings { get; set; }
        private IEnumerable<int> AccountsList { get; set; }
        private IEnumerable<MeterReadingBase> PreviousMeterReadingsList { get; set; }

        private readonly string validAccountIDRegEx = "^[0-9]{5}$";

        public MeterReadingValidator(IEnumerable<MeterReadingBase> meterReadingsToValidate, IEnumerable<int> accounts, IEnumerable<MeterReadingBase> previousmeterreadings)
        {
            MeterReadings = meterReadingsToValidate;
            AccountsList = accounts;
            PreviousMeterReadingsList = previousmeterreadings;
        }

        public void ValidateAll()
        {
            ValidateMeterReadingValue();
            ValidateAccountID();
            ValidateIfReadingAlreadyInDB();
            ValidateIfReadingIsNewerThanLatestInDB();
        }

        public void ValidateMeterReadingValue()
        {
            System.Text.RegularExpressions.Regex rx = new(validAccountIDRegEx);
            MeterReadings
                .Where(x => !rx.IsMatch(x.MeterReadValue) ).ToList()
                .ForEach(meterReading => {
                    meterReading.IsValid = false;
                    meterReading.ValidationError = "meter reading value is invalid";
                });
        }

        public void ValidateAccountID()
        {
            var invalidAccounts = MeterReadings
                .Select(x => x.AccountID)
                .Except(AccountsList);

            //.net6 has ExceptBy LINQ function which would help avoid the below
            foreach (var meterReading in MeterReadings)
            {
                foreach (var accountID in invalidAccounts)
                {
                    if (meterReading.AccountID == accountID)
                    {
                        meterReading.IsValid = false;
                        meterReading.ValidationError = "accountID invalid";
                    }
                }
            }

        }

        public void ValidateIfReadingAlreadyInDB()
        {
            MeterReadings
                .Intersect(PreviousMeterReadingsList).ToList() //get the readings that already exists in previous readings list
                .ForEach(meterReading => {
                    meterReading.IsValid = false;
                    meterReading.ValidationError = "meter reading already in database";
                });
        }

        public void ValidateIfReadingIsNewerThanLatestInDB()
        {
            foreach (var meterReading in MeterReadings)
            {
                foreach (var previousMeterReading in PreviousMeterReadingsList)
                {
                    if (meterReading.AccountID == previousMeterReading.AccountID 
                        && meterReading.MeterReadingDateTime <= previousMeterReading.MeterReadingDateTime)
                    {
                        meterReading.IsValid = false;
                        meterReading.ValidationError = "a newer entry alrady exists";
                    }
                }
            }
        }
    }
}
