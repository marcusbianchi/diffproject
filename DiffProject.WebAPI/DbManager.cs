using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiffProject.WebAPI
{
    public class DbManager
    {
        private readonly IConfiguration _configuration;
        public DbManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateDb()
        {
            var dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)  + _configuration.GetConnectionString("ProcessContext");
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            var dbConnection = new SQLiteConnection(string.Format(
                "Data Source={0};Version=3;", dbFilePath));
            CreateTables(dbConnection);
            return dbConnection;
        }

        public void CreateTables(IDbConnection dbConnection)
        {
            using (var connection = dbConnection)
            {
                string sql = @"
                CREATE TABLE IF NOT EXISTS ItemToProcess (
                    ItemToProcessId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    ContentId TEXT NULL,
                    Direction TEXT  NULL,
                    Size INTEGER NULL, 
                    Hash TEXT  NULL
                );
                CREATE TABLE IF NOT EXISTS ProcessResult (
                    ProcessResultId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    ContentId TEXT NULL,
                    IsEqual BOOLEAN NULL,
                    IsEqualSize BOOLEAN NULL,
                    status INTEGER NULL
                );";
                connection.Execute(sql);
            }
        }
    }
}