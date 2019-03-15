using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IComparisonRepository
    {
        ProcessResult SaveResult(ProcessResult processResult);
        ProcessResult UpdateResultByContentId(ProcessResult processResult,string contentId);
        ProcessResult GetResultByContentId(string contentId);

    }
}
