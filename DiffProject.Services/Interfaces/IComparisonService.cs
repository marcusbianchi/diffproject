using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IComparisonService
    {
        ProcessResult SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft);
        ProcessResult CreateNewComparison(string contentId);
        ProcessResult UpdateComparisonToProcessing(string contentId, StatusEnum status);
    }
}
