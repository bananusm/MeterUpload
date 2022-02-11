using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload.MeterReadingModels
{
    public class Customer2MeterReading : MeterReadingBase
    {
        public string ExtraData { get; set; }

        public Customer2MeterReading(int accountID, string datetime, string value, string extradata) : base(accountID, datetime, value)
        {
            this.ExtraData = extradata;
        }

        public Customer2MeterReading() { }

        public override bool Equals(object obj)
        {
            return obj is Customer2MeterReading reading &&
                   AccountID == reading.AccountID &&
                   MeterReadingDateTime == reading.MeterReadingDateTime &&
                   MeterReadValue == reading.MeterReadValue &&
                   ExtraData == reading.ExtraData;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountID, MeterReadingDateTime, MeterReadValue, ExtraData);
        }
    }
}
