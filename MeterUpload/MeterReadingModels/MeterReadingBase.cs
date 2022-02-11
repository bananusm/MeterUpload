using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload
{
    public abstract class MeterReadingBase : IValidatable
    {
        public int AccountID { get; init; }

        public DateTime MeterReadingDateTime { get; init; }

        public string MeterReadValue { get; init; }

        public bool IsValid { get; set; } = true;

        public string ValidationError { get; set; }

        public MeterReadingBase(int accountID, string datetime, string value)
        {
            this.AccountID = accountID;
            this.MeterReadingDateTime = DateTime.Parse(datetime);
            this.MeterReadValue = value;
        }

        public MeterReadingBase(){}

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();
    }
}
