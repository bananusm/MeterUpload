using NUnit.Framework;
using MeterUpload;
using MeterUpload.MeterReadingModels;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Microsoft.Data.Sqlite;
using Dapper;

namespace UnitTests
{
    [TestFixture]
    [Explicit("clears the db")]
    class DBTests
    {

        public string DBFile;
        public SqliteConnection conn;

        [SetUp]
        public void Init()
        {
            DBFile = DB.DbFile;
            conn = DB.SimpleDbConnection();
            conn.Open();
        }

        [TearDown]
        public void End()
        {
            conn.Close();
        }

        [Test]
        public void ClearDBEntries()
        {
            if (!File.Exists(DBFile)) throw new FileNotFoundException("database file not found");

            conn.Execute(@"DELETE FROM MeterReadings");
        }
    }
}
