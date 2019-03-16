using Dapper;
using Microsoft.AspNetCore.Hosting;
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
    /// <summary>
    /// Class to create a new SQLLiteDBForTheProject
    /// </summary>
    public class DbManager
    {
        /// <summary>
        /// Acceess to Config locations
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        /// <summary>
        /// Construct the Manager to create DB
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public DbManager(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        /// <summary>
        /// Create the DB File 
        /// </summary>
        public void CreateDb()
        {
            var dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)  + _configuration.GetConnectionString("ProcessContext");
            if (_env.IsDevelopment())
            {
                File.Delete(dbFilePath);
            }
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            var dbConnection = new SQLiteConnection(string.Format(
                "Data Source={0};Version=3;", dbFilePath));
            CreateTables(dbConnection);
        }


        /// <summary>
        /// Create the Required Tables for the Project
        /// </summary>
        /// <param name="dbConnection">Object to Connect to the Database</param>
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