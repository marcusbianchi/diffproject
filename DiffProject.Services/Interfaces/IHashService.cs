
namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Service to Calculate Hash Values of string
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        /// Receives the Raw input and convert to Hash Value
        /// </summary>
        /// <param name="input">Raw Input</param>
        /// <returns>Hash Value</returns>
        string CreateHash(string input);
    }
}
