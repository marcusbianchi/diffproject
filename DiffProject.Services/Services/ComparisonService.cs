using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;

namespace DiffProject.Services.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly IProcessResultRepository _comparisonRepository;
        private readonly IDiffenceSearchService _diffenceSearchService;
        public ComparisonService(IProcessResultRepository comparisonRepository
            , IDiffenceSearchService diffenceSearchService)
        {
            _comparisonRepository = comparisonRepository;
            _diffenceSearchService = diffenceSearchService;
        }
        public ProcessResult CreateNewComparison(string contentId)
        {
            //Creates a new Process result
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
            //if same size find the difference
            if (processResult.IsEqualSize)
            {
                var diff = _diffenceSearchService.GetDifferences(itemToProcessRight.Text, itemToProcessLeft.Text, itemToProcessRight.Size);
                processResult.Differences = diff;
            }
            processResult.status = StatusEnum.DONE;
            return _comparisonRepository.UpdateResultByContentId(processResult, itemToProcessRight.ContentId);
        }

        public ProcessResult UpdateComparisonToProcessing(string contentId, StatusEnum status)
        {
            //Change a processing status
            var processResult = _comparisonRepository.GetResultByContentId(contentId);
            processResult.status = status;
            return _comparisonRepository.UpdateResultByContentId(processResult, contentId);
        }
    }
}
