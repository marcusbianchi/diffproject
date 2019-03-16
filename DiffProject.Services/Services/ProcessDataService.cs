using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Services.Services
{
    public class ProcessDataService : IProcessDataService
    {
        private readonly IItemToProcessRepository _itemToProcessRepository;
        private readonly IProcessResultRepository _comparisonRepository;
        private readonly IHashService _hashService;
        private readonly IComparisonService _comparisonService;
        public ProcessDataService(IItemToProcessRepository itemToProcessRepository, IHashService hashService,
            IComparisonService comparisonService, IProcessResultRepository comparisonRepository)
        {
            _itemToProcessRepository = itemToProcessRepository;
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
                Size = content.Length,
                Text = content
            };
            var currenResult = _comparisonRepository.GetResultByContentId(contentId);
            var itemOnDb = await _itemToProcessRepository.GetDataFromDbById(contentId);
            //If no item in the DB create new Process
            if (currenResult == null)
            {
                SaveNewContentToDb(content, contentId, itemToProcess);
                return true;
            }
            //Process if already in DB
            if ((itemOnDb.Direction == "right" && direction == "left") || (itemOnDb.Direction == "left" && direction == "right"))
            {
                return ProcessContentAlreadyOnDb(content, contentId, direction, itemToProcess, itemOnDb);
            }
            return false;
        }

        private void SaveNewContentToDb(string content, string contentId, ItemToProcess itemToProcess)
        {
            //Save the Id in the data base and start the processing of new item async
            _comparisonService.CreateNewComparison(contentId);
            new Thread(() =>
            {
                itemToProcess.Hash = _hashService.CreateHash(content);
                _comparisonService.CreateNewComparison(contentId);
                _itemToProcessRepository.SaveDataToDB(itemToProcess);
                _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.PROCESSED_FIRST);
            }).Start();
        }

        private bool ProcessContentAlreadyOnDb(string content, string contentId, string direction, ItemToProcess itemToProcess, ItemToProcess itemOnDb)
        {
            var currenResult = _comparisonRepository.GetResultByContentId(contentId);
            //If item already done returns if new start thread
            if (currenResult.status == StatusEnum.NEW || currenResult.status == StatusEnum.PROCESSED_FIRST)
            {
                _comparisonService.UpdateComparisonToProcessing(contentId, StatusEnum.PROCESSED_SECOND_STARTED);
                new Thread(() =>
                {
                    //wait while the previuos value is processed
                    while (currenResult.status != StatusEnum.PROCESSED_FIRST)
                    {
                        currenResult = _comparisonRepository.GetResultByContentId(contentId);
                        Thread.Sleep(100);
                    }
                    //if same size calculate the hash to compare contents
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
                return true;
            }
            return false;
        }


    }
}
