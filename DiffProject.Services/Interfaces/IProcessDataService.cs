using DiffProject.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    public interface IProcessDataService
    {
        Task<bool> ProcessDataAsync(string content,string contentId, string direction);
    }
}
