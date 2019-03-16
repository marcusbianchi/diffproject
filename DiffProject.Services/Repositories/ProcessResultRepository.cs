using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace DiffProject.Service.Repositories
{
    public class ProcessResultRepository : IProcessResultRepository
    {
        private readonly string _dbFilePath;
        public ProcessResultRepository(IConfiguration configuration)
        {
            _dbFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                + configuration.GetConnectionString("ProcessContext");
        }
        public ProcessResult GetResultByContentId(string contentId)
        {
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath)))
            {
                string selectQuery = "SELECT ProcessResultId, ContentId, status, IsEqual, IsEqualSize,DifferencesSerialized " +
                    "FROM ProcessResult WHERE contentId = @contentId";
                conn.Open();
                var result = conn.Query<ProcessResult>(selectQuery, new { contentId = contentId }).FirstOrDefault(); ;
                if(!String.IsNullOrEmpty(result?.DifferencesSerialized))
                    result.Differences = JsonConvert.DeserializeObject<IList<Difference>>(result.DifferencesSerialized);
                return result;
            }
        }

        public ProcessResult SaveResult(ProcessResult processResult)
        {
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath)))
            {
                string insertQuery = @"INSERT INTO ProcessResult ([ContentId], [status], [IsEqual], [IsEqualSize]) VALUES (@ContentId, @status, @IsEqual, @IsEqualSize)";
                conn.Open();
                var result = conn.Execute(insertQuery, processResult);
                return GetResultByContentId(processResult.ContentId);
            }
        }

        public ProcessResult UpdateResultByContentId(ProcessResult processResult, string contentId)
        {
            if(processResult.Differences != null)
                processResult.DifferencesSerialized = JsonConvert.SerializeObject(processResult.Differences);
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _dbFilePath)))
            {
                string updateQuery = @"UPDATE ProcessResult SET ContentId = @ContentId, status = @status , IsEqual = @IsEqual, IsEqualSize = @IsEqualSize, DifferencesSerialized = @DifferencesSerialized  WHERE contentId = " + contentId;
                conn.Open();
                var result = conn.Execute(updateQuery, processResult);
                return GetResultByContentId(contentId);
            }
        }
    }
}
