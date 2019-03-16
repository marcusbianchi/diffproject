using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Manages the processing of data
    /// </summary>
    public interface IProcessDataService
    {
        /// <summary>
        /// Process the data coming throught the database connection.
        /// </summary>
        /// <param name="content">Body of the Request</param>
        /// <param name="contentId">Id of the request</param>
        /// <param name="direction">"left" or "right"</param>
        /// <returns>A task that returns if the process was properly run.</returns>
        Task<bool> ProcessDataAsync(string content,string contentId, string direction);
    }
}
