using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterUpload
{
    public class ETL
    {

        public string[] getCSVlinesFromPostbody(string postbody, string jsonVariableName)
        {
            var postdata = JObject.Parse(postbody.ToString());
            var txt = postdata[jsonVariableName].ToString();
            return txt.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[1..]; //excludes first (header) line
        }

        public IEnumerable<MeterReadingBase> getMeterReadingsFromCSVlines(string[] csvlines, Func<string[],MeterReadingBase> createMeterReadingFunc)
        {
            List<MeterReadingBase> mrlist = new();
            foreach (string line in csvlines) //skip first line with headers
            {
                string[] columns = line.Split(",");
                var meterReading = createMeterReadingFunc(columns);
                mrlist.Add(meterReading);
            }
            return mrlist.Distinct(); //exclude duplicates
        }

         


}
}
