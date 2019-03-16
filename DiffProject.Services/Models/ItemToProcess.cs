using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiffProject.Services.Models
{
    /// <summary>
    /// Holds the data to be processed
    /// </summary>
    public class ItemToProcess
    {
        /// <summary>
        /// Databasae Id
        /// </summary>
        public int ItemToProcessId { get; set; }        

        /// <summary>
        /// Content Id as sent from the http request
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Direction as sent from the http request
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Content Size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Content Hash
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Raw value
        /// </summary>
        public string Text { get; set; }

    }
}
