using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiffProject.Services.Models
{
    [Table("ItemToProcess")]
    public class ItemToProcess
    {
        [ExplicitKey]
        public int ItemToProcessId { get; set; }        
        public string ContentId { get; set; }
        public string Direction { get; set; }
        public int Size { get; set; }
        public string Hash { get; set; }
    }
}
