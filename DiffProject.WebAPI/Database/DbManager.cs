using Dapper;
using DiffProject.Services.Interfaces;
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

namespace DiffProject.WebAPI.Database
{
    /// <summary>
    /// Class to create a new SQLLiteDBForTheProject
    /// </summary>
    public class DbManager : IDbManager
    {
        /// <summary>
        /// Acceess to Config locations
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Construct the Manager to create DB
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>

        public DbManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateDb(IHostingEnvironment env)
        {
            var dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + _configuration.GetConnectionString("ProcessContext");

            //Delete previous data if in dev mode
            if (env.IsDevelopment())
            {
                File.Delete(dbFilePath);
            }

            //create new DB file
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            var dbConnection = new SQLiteConnection(string.Format(
                "Data Source={0};Version=3;", dbFilePath));
            CreateTables(dbConnection);
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
                    Hash TEXT  NULL,
                    Text TEXT  NULL

                );
                CREATE TABLE IF NOT EXISTS ProcessResult (
                    ProcessResultId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    ContentId TEXT NULL,
                    DifferencesSerialized TEXT NULL,
                    IsEqual BOOLEAN NULL,
                    IsEqualSize BOOLEAN NULL,
                    status INTEGER NULL
                );";
                connection.Execute(sql);
            }
        }


        public IDbConnection CreateConnection()
        {
            var _dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                + _configuration.GetConnectionString("ProcessContext");
            return new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath));
        }
    }
}