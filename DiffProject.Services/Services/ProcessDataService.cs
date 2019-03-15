using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Services.Services
{
    public class ProcessDataService : IProcessDataService
    {
        private readonly IDataRepository _dataRepository;
        private readonly IComparisonRepository _comparisonRepository;
        private readonly IHashService _hashService;
        private readonly IComparisonService _comparisonService;
        public ProcessDataService(IDataRepository dataRepository, IHashService hashService,
            IComparisonService comparisonService, IComparisonRepository comparisonRepository)
        {
            _dataRepository = dataRepository;
            _hashService = hashService;
            _comparisonService = comparisonService;
            _comparisonRepository = comparisonRepository;
        }
        public async Task<bool> ProcessDataAsync(string content, string contentId, string direction)
        {
            var itemToProcess = new ItemToProcess
            {
                ContentId = contentId,
                Direction = direction,
                Size = content.Length
            };
            var currenResult = _comparisonRepository.GetResultByContentId(contentId);
            var itemOnDb = await _dataRepository.GetDataFromDbById(contentId);
            if (currenResult == null)
            {
                SaveNewContentToDb(content, contentId, itemToProcess);
                return true;
            }
            if ((itemOnDb.Direction == "right" && direction == "left") || (itemOnDb.Direction == "left" && direction == "right"))
            {
                ProcessContentAlreadyOnDb(content, contentId, direction, itemToProcess, itemOnDb);
                return true;
            }
            return false;
        }

        private void SaveNewContentToDb(string content, string contentId, ItemToProcess itemToProcess)
        {
            _comparisonService.CreateNewComparison(contentId);
            new Thread(() =>
            {
                itemToProcess.Hash = _hashService.CreateHash(content);
                _comparisonService.CreateNewComparison(contentId);
                _dataRepository.SaveDataToDB(itemToProcess);
                _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.PROCESSED_FIRST);
            }).Start();
        }

        private void ProcessContentAlreadyOnDb(string content, string contentId, string direction, ItemToProcess itemToProcess, ItemToProcess itemOnDb)
        {
            var currenResult = _comparisonRepository.GetResultByContentId(contentId);
            if (currenResult.status == StatusEnum.NEW || currenResult.status == StatusEnum.PROCESSED_FIRST)
            {
                while (currenResult.status != StatusEnum.PROCESSED_FIRST)
                {
                    currenResult = _comparisonRepository.GetResultByContentId(contentId);
                    Thread.Sleep(100);
                }
                _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.PROCESSED_SECOND_STARTED);
                new Thread(() =>
                       {
                           if (itemToProcess.Size == itemOnDb.Size)
                               itemToProcess.Hash = _hashService.CreateHash(content);
                           else
                               itemToProcess.Hash = "";
                           if (itemOnDb.Direction == "right" && direction == "left")
                           {
                               _comparisonService.SaveProcessResult(itemOnDb, itemToProcess);
                               _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.DONE);
                           }
                           if (itemOnDb.Direction == "left" && direction == "right")
                           {
                               _comparisonService.SaveProcessResult(itemToProcess, itemOnDb);
                               _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.DONE);
                           }

                       }).Start();
            }
        }


    }
}
