using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Services
{
    public class ProcessDataService : IProcessDataService
    {
        private readonly IDataRepository _dataRepository;
        private readonly IHashService _hashService;
        private readonly IComparisonService _comparisonService;
        public ProcessDataService(IDataRepository dataRepository, IHashService hashService, IComparisonService comparisonService)
        {
            _dataRepository = dataRepository;
            _hashService = hashService;
            _comparisonService = comparisonService;
        }
        public async Task<bool> ProcessDataAsync(string content, string contentId, string direction)
        {
            var itemToProcess = new ItemToProcess
            {
                Hash = _hashService.CreateHash(content),
                ContentId = contentId,
                Direction = direction,
                Size = content.Length
            };
            var itemOnDb = await _dataRepository.GetDataFromDbById(contentId);
            if (itemOnDb == null)
            {
                await _comparisonService.CreateNewComparisonAsync(content);
                await _dataRepository.SaveDataToDB(itemToProcess);
                return true;
            }
            if (itemOnDb.Direction == "right" && direction == "left")
            {
                await _comparisonService.UpdateComparisonToProcessingAsync(content);
                _ = Task.Run(() => _comparisonService.SaveProcessResult(itemOnDb, itemToProcess));
                return true;
            }
            if (itemOnDb.Direction == "left" && direction == "right")
            {
                await _comparisonService.UpdateComparisonToProcessingAsync(content);
                _ = Task.Run(() => _comparisonService.SaveProcessResult(itemOnDb, itemToProcess));
                return true;
            }
            return false;
        }

    }
}
