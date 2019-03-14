using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiffProject.Services.Models
{
    public class ProcessContext : DbContext
    {
        public ProcessContext(DbContextOptions<ProcessContext> options)
            : base(options)
        { }
        public DbSet<ItemToProcess> ItemsToProcess { get; set; }
        public DbSet<ProcessResult> Posts { get; set; }
    }
}
