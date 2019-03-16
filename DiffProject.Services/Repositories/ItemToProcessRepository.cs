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
using System.IO;
using System.Reflection;

namespace DiffProject.Service.Repositories
{
    public class ItemToProcessRepository : IItemToProcessRepository
    {
        private readonly string _dbFilePath;
        public ItemToProcessRepository(IConfiguration configuration)
        {
            _dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                + configuration.GetConnectionString("ProcessContext");
        }

        public async Task<ItemToProcess> GetDataFromDbById(string ContentId)
        {
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath)))
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
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath)))
            {
                string insertQuery = @"INSERT INTO ItemToProcess ([ContentId], [Direction], [Size],[Hash]) VALUES ( @ContentId, @Direction, @Size, @Hash)";
                conn.Open();
                var result = conn.Execute(insertQuery, itemToProcess);
                var identity = await conn.InsertAsync(itemToProcess);
            }
        }
    }
}
