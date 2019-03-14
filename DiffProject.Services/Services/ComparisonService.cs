using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly IComparisonRepository _comparisonRepository;
        public ComparisonService(IComparisonRepository comparisonRepository)
        {
            _comparisonRepository = comparisonRepository;
        }
        public async Task<ProcessResult> CreateNewComparisonAsync(string contentId)
        {
            var processResult = new ProcessResult
            {
                ContentId = contentId,
                status = StatusEnum.NEW
            };
            return await _comparisonRepository.SaveResult(processResult);
        }

        public async Task<ProcessResult> SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft)
        {
            var processResult = await _comparisonRepository.GetResultByContentId(itemToProcessRight.ContentId);
            processResult.IsEqual = itemToProcessRight.Hash == itemToProcessLeft.Hash;
            processResult.IsEqualSize = itemToProcessRight.Size == itemToProcessLeft.Size;
            processResult.status = StatusEnum.DONE;
            return await _comparisonRepository.UpdateResultByContentId(processResult, itemToProcessRight.ContentId);
        }

        public async Task<ProcessResult> UpdateComparisonToProcessingAsync(string contentId)
        {
            var processResult = await _comparisonRepository.GetResultByContentId(contentId);
            processResult.status = StatusEnum.PROCESSING;
            return await _comparisonRepository.UpdateResultByContentId(processResult, contentId);
        }
    }
}
