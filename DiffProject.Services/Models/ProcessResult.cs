using Newtonsoft.Json;
using System.Collections.Generic;


namespace DiffProject.Services.Models
{
    public class ProcessResult
    {
        public int ProcessResultId { get; set; }
        public string ContentId { get; set; }
        public StatusEnum status { get; set; }
        public bool IsEqual { get; set; }
        public bool IsEqualSize { get; set; }
        public IList<Difference> Differences { get; set; }
        [JsonIgnore]
        public string DifferencesSerialized { get; set; }

    }
}
