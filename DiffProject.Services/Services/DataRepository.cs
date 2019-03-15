using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Data.SQLite;

namespace DiffProject.Service.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly IConfiguration _configuration;
        public DataRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ItemToProcess> GetDataFromDbById(string ContentId)
        {

            var dbFilePath = _configuration.GetConnectionString("ProcessContext");
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath)))
            {
                string sQuery = "SELECT ItemToProcessId, ContentId, Direction, Size, Hash " +
                    "FROM ItemToProcess WHERE contentId = @contentId";
                conn.Open();
                var result = await conn.QueryAsync<ItemToProcess>(sQuery, new { contentId = ContentId });
                return result.FirstOrDefault();
            }
        }

        public async Task SaveDataToDB(ItemToProcess itemToProcess)
        {
            var dbFilePath = _configuration.GetConnectionString("ProcessContext");
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath)))
            {
                string insertQuery = @"INSERT INTO ItemToProcess ([ContentId], [Direction], [Size],[Hash]) VALUES ( @ContentId, @Direction, @Size, @Hash)";
                conn.Open();
                var result = conn.Execute(insertQuery, itemToProcess);
                var identity = await conn.InsertAsync(itemToProcess);
            }
        }
    }
}
