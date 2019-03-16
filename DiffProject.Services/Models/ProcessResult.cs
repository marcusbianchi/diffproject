using Newtonsoft.Json;
using System.Collections.Generic;


namespace DiffProject.Services.Models
{
    /// <summary>
    /// Holds the data after processing
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public int ProcessResultId { get; set; }

        /// <summary>
        /// Content Id as sent from the http request
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Status of the procesing
        /// </summary>
        public StatusEnum status { get; set; }

        /// <summary>
        /// If the two string where equal
        /// </summary>
        public bool IsEqual { get; set; }

        /// <summary>
        /// If the two string had the same size
        /// </summary>
        public bool IsEqualSize { get; set; }

        /// <summary>
        /// The differences found
        /// </summary>
        public IList<Difference> Differences { get; set; }

        /// <summary>
        /// The diferences as string to be saved on the database
        /// </summary>
        [JsonIgnore]
        public string DifferencesSerialized { get; set; }

    }
}
