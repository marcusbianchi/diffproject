using DiffProject.Services.Models;
using System.Collections.Generic;

namespace DiffProject.Services.Interfaces
{
    public interface IDiffenceSearchService
    {
       IList<Difference> GetDifferences(string first, string second, int size);
    }
}
