using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload
{
    public interface IValidatable
    {
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }
}
