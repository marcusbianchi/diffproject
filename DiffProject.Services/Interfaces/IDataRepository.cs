using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IDataRepository
    {
        Task<ItemToProcess> GetDataFromDbById(string ContentId);
        Task SaveDataToDB(ItemToProcess itemToProcess);
    }
}
