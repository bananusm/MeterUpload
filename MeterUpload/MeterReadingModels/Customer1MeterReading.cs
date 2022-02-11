using System;

namespace MeterUpload
{
    public class Customer1MeterReading : MeterReadingBase
    {

        public Customer1MeterReading(int accountID, string datetime, string value) : base(accountID,datetime,value)
        {
        }

        public Customer1MeterReading(){}

        public override bool Equals(object obj)
        {
            return obj is MeterReadingBase @base &&
                   AccountID == @base.AccountID &&
                   MeterReadingDateTime == @base.MeterReadingDateTime &&
                   MeterReadValue == @base.MeterReadValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountID, MeterReadingDateTime, MeterReadValue);
        }
    }

   
}
