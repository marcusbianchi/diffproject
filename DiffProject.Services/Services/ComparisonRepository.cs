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
using Dapper.Contrib.Extensions;
using System.Data.SQLite;

namespace DiffProject.Service.Services
{
    public class ComparisonRepository : IComparisonRepository
    {
        private readonly IConfiguration _configuration;
        public ComparisonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ProcessResult GetResultByContentId(string contentId)
        {
            var dbFilePath = _configuration.GetConnectionString("ProcessContext");
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath)))
            {
                string selectQuery = "SELECT ProcessResultId, ContentId, status, IsEqual, IsEqualSize " +
                    "FROM ProcessResult WHERE contentId = @contentId";
                conn.Open();
                var result = conn.Query<ProcessResult>(selectQuery, new { contentId = contentId });
                return result.FirstOrDefault();
            }
        }

        public ProcessResult SaveResult(ProcessResult processResult)
        {
            var dbFilePath = _configuration.GetConnectionString("ProcessContext");
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath)))
            {
                string insertQuery = @"INSERT INTO ProcessResult ([ContentId], [status], [IsEqual], [IsEqualSize]) VALUES (@ContentId, @status, @IsEqual, @IsEqualSize)";
                conn.Open();
                var result = conn.Execute(insertQuery, processResult);
                return GetResultByContentId(processResult.ContentId);
            }
        }

        public ProcessResult UpdateResultByContentId(ProcessResult processResult, string contentId)
        {
            var dbFilePath = _configuration.GetConnectionString("ProcessContext");
            using (IDbConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath)))
            {
                string updateQuery = @"UPDATE ProcessResult SET ContentId = @ContentId, status = @status , IsEqual = @IsEqual, IsEqualSize = @IsEqualSize  WHERE contentId = " + contentId;
                conn.Open();
                var result = conn.Execute(updateQuery, processResult);
                return GetResultByContentId(contentId); ;
            }
        }
    }
}
