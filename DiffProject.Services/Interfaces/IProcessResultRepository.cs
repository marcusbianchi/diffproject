using DiffProject.Services.Models;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Interface to Manage the Comparison between files.
    /// </summary>
    public interface IProcessResultRepository
    {
        /// <summary>
        /// Save processing result and returns with the ID
        /// </summary>
        /// <param name="processResult">Processing Result without the ID</param>
        /// <returns>Processing Result with the ID</returns>
        ProcessResult SaveResult(ProcessResult processResult);

        /// <summary>
        /// Update the Result of the processing
        /// </summary>
        /// <param name="processResult">New process Result Data</param>
        /// <param name="contentId">Id of the Content to be updated</param>
        /// <returns>Processing Result with the new values</returns>
        ProcessResult UpdateResultByContentId(ProcessResult processResult,string contentId);

        /// <summary>
        /// Return the current result for the contentId
        /// </summary>
        /// <param name="contentId">Id of the file</param>
        /// <returns>Processing Result</returns>
        ProcessResult GetResultByContentId(string contentId);

    }
}
