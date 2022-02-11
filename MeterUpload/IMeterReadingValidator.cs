using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload
{
    interface IMeterReadingValidator
    {
        public void ValidateAll();
        public void ValidateMeterReadingValue();
        public void ValidateAccountID();
        public void ValidateIfReadingAlreadyInDB();
        public void ValidateIfReadingIsNewerThanLatestInDB();
    }
}
