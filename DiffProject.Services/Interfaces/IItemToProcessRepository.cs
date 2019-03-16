using DiffProject.Services.Models;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IItemToProcessRepository
    {
        Task<ItemToProcess> GetDataFromDbById(string ContentId);
        Task SaveDataToDB(ItemToProcess itemToProcess);
    }
}
