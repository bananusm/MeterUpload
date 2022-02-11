using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;


namespace MeterUpload.Controllers
{
    [ApiController]
    [Route("/")]
    public class MeterUploadController : ControllerBase
    {
        private readonly ILogger<MeterUploadController> _logger;

        public MeterUploadController(ILogger<MeterUploadController> logger)
        {
            _logger = logger;
        }

        [HttpPost("meter-reading-uploads")]
        public JsonResult Post([FromBody] object postbody)
        {
            int validReadingsCnt;
            int invalidReadingsCnt;
            try
            {
                ETL etl = new();  //utility classes for CSV
                DB  db  = new();  //and database

                //get an array of each line from csv
                string[] csvlines = etl.getCSVlinesFromPostbody(postbody.ToString(),"csv");

                //function for creating Meter reading for this particular customer
                static Customer1MeterReading createCustomersMeterReadingFunc(string[] csvcolumns) => new(int.Parse(csvcolumns[0]), csvcolumns[1], csvcolumns[2]);

                //get meter readings
                List<MeterReadingBase> mrlist = etl.getMeterReadingsFromCSVlines(csvlines, createCustomersMeterReadingFunc).ToList();

                //get lists for validations
                IEnumerable<int> existingAccountIDs = db.GetAccounts();
                IEnumerable<Customer1MeterReading> previousMeterReadings = db.Customer1GetPreviousMeterReadings();   ///get previous meter readings

                //validate
                var validator = new MeterReadingValidator(mrlist, existingAccountIDs, previousMeterReadings);
                validator.ValidateAll();

                //save to database
                var affectedrows = db.SaveMeterReadingsToDB(mrlist.Where(x => x.IsValid));
                
                //count success and fails
                validReadingsCnt = affectedrows;
                invalidReadingsCnt = mrlist.Count - affectedrows;
            }
            catch (FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine($"exception {ex} - invalid csv accountID");
                validReadingsCnt = -1;
                invalidReadingsCnt = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"exception {ex}");
                validReadingsCnt = -1;
                invalidReadingsCnt = -1;
            }

            return new JsonResult(new { successful = validReadingsCnt, failed = invalidReadingsCnt });
        }
    }
}
