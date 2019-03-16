using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;

namespace DiffProject.Services.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly IProcessResultRepository _comparisonRepository;
        public ComparisonService(IProcessResultRepository comparisonRepository)
        {
            _comparisonRepository = comparisonRepository;
        }
        public ProcessResult CreateNewComparison(string contentId)
        {
            var processResult = new ProcessResult
            {
                ContentId = contentId,
                status = StatusEnum.NEW
            };
            return  _comparisonRepository.SaveResult(processResult);
        }

        public ProcessResult SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft)
        {
            var processResult = _comparisonRepository.GetResultByContentId(itemToProcessRight.ContentId);
            processResult.IsEqual = itemToProcessRight.Hash == itemToProcessLeft.Hash;
            processResult.IsEqualSize = itemToProcessRight.Size == itemToProcessLeft.Size;
            processResult.status = StatusEnum.DONE;
            return _comparisonRepository.UpdateResultByContentId(processResult, itemToProcessRight.ContentId);
        }

        public ProcessResult UpdateComparisonToProcessing(string contentId, StatusEnum status)
        {
            var processResult = _comparisonRepository.GetResultByContentId(contentId);
            processResult.status = status;
            return _comparisonRepository.UpdateResultByContentId(processResult, contentId);
        }
    }
}
