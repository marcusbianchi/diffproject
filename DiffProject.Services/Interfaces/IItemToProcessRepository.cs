using DiffProject.Services.Models;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Save the data to be processed further
    /// </summary>
    public interface IItemToProcessRepository
    {
        /// <summary>
        /// Get a previous sent item from the Database
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        /// <returns>A task to retrieve the item</returns>
        Task<ItemToProcess> GetDataFromDbById(string ContentId);
        /// <summary>
        /// Save a new item to the Database
        /// </summary>
        /// <param name="itemToProcess">Item to be saved</param>
        /// <returns>A task to send the item</returns>
        Task SaveDataToDB(ItemToProcess itemToProcess);
    }
}
