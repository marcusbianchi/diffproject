using System;
using System.Collections.Generic;
using System.Text;

namespace DiffProject.Services.Models
{
    public class ProcessResult
    {
        public int ProcessResultId { get; set; }
        public string ContentId { get; set; }
        public StatusEnum status { get; set; }
        public bool IsEqual { get; set; }
        public bool IsEqualSize { get; set; }
        //diffs

    }
}
