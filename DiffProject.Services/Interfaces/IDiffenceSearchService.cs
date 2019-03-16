using DiffProject.Services.Models;
using System.Collections.Generic;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Service to find the difference between strings
    /// </summary>
    public interface IDiffenceSearchService
    {
        /// <summary>
        /// Search the difference between 2 strings and return their points of difference
        /// </summary>
        /// <param name="first">First String to Compare</param>
        /// <param name="second">Second String to Compare</param>
        /// <param name="size">Total Size of the String</param>
        /// <returns>The List of the differences</returns>
        IList<Difference> GetDifferences(string first, string second, int size);
    }
}
