using DiffProject.Services.Models;

namespace DiffProject.Services.Interfaces
{
    public interface IComparisonService
    {
        ProcessResult SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft);
        ProcessResult CreateNewComparison(string contentId);
        ProcessResult UpdateComparisonToProcessing(string contentId, StatusEnum status);
    }
}
