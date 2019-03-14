using System;
using System.Collections.Generic;
using System.Text;

namespace DiffProject.Services.Models
{
    public class ItemToProcess
    {
        public int ItemToProcessId { get; set; }        
        public string ContentId { get; set; }
        public string Direction { get; set; }
        public int Size { get; set; }
        public string Hash { get; set; }
    }
}
