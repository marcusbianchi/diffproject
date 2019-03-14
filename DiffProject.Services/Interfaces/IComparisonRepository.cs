using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IComparisonRepository
    {
        Task<ProcessResult> SaveResult(ProcessResult processResult);
        Task<ProcessResult> UpdateResultByContentId(ProcessResult processResult,string contentId);
        Task<ProcessResult> GetResultByContentId(string contentId);

    }
}
