using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MeterUpload
{
    public class DB
    {
   
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\energy.db"; }
        }

        public static SqliteConnection SimpleDbConnection()
        {
            return new Microsoft.Data.Sqlite.SqliteConnection("Data Source=" + DbFile);
        }

        public IEnumerable<int> GetAccounts()
        {
            if (!File.Exists(DbFile)) throw new FileNotFoundException("database file not found");

            using var cnn = SimpleDbConnection();
            return cnn.Query<int>(@"SELECT AccountID FROM Accounts").ToList();
        }

        public IEnumerable<Customer1MeterReading> Customer1GetPreviousMeterReadings()
        {
            if (!File.Exists(DbFile)) throw new FileNotFoundException("database file not found");

            using var cnn = SimpleDbConnection();
            return cnn.Query<Customer1MeterReading>(@"SELECT AccountID, MeterReadingDateTime, MeterReadValue FROM MeterReadings").ToList();
        }

        public int SaveMeterReadingsToDB(IEnumerable<MeterReadingBase> validatedReadingsList )
        {
            if (!File.Exists(DbFile)) throw new FileNotFoundException("database file not found");

            using var cnn = SimpleDbConnection();
            int affectedRows = 0;

            foreach (var reading in validatedReadingsList)
            {

                affectedRows += cnn.Execute(@"INSERT INTO MeterReadings(AccountID,MeterReadingDateTime,MeterReadValue ) 
                                              VALUES (@AccountID, @MeterReadingDateTime, @MeterReadValue)", 
                                              new {
                                                  reading.AccountID,
                                                  reading.MeterReadingDateTime,
                                                  reading.MeterReadValue
                                              });
            
            }

            return affectedRows;
        }

    }
}
