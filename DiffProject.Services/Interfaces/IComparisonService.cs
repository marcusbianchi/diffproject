using DiffProject.Services.Models;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Interface to Compare the payload received
    /// </summary>
    public interface IComparisonService
    {
        /// <summary>
        /// Process the Result of the call and put it in the database for consulting
        /// </summary>
        /// <param name="itemToProcessRight">Left String</param>
        /// <param name="itemToProcessLeft">Right String</param>
        /// <returns>The Process Result</returns>
        ProcessResult SaveProcessResult(ItemToProcess itemToProcessRight, ItemToProcess itemToProcessLeft);
        /// <summary>
        /// Save a new comparison to the database
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        /// <returns>The Comparison with the new state</returns>
        ProcessResult CreateNewComparison(string contentId);
        /// <summary>
        /// Update a comparison to new status
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        /// <param name="status">New Status</param>
        /// <returns>The Comparison with the new state</returns>
        ProcessResult UpdateComparisonToProcessing(string contentId, StatusEnum status);
    }
}
