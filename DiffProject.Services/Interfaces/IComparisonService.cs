using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IComparisonService
    {
        void SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft);

        Task<ProcessResult> CreateNewComparisonAsync(string contentId);
        Task<ProcessResult> UpdateComparisonToProcessingAsync(string contentId);
    }
}
