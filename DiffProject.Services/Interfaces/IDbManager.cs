using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DiffProject.Services.Interfaces
{
    /// <summary>
    /// Class to create a new DB for the Project
    /// </summary>
    public interface IDbManager
    {

        /// <summary>
        /// Create the DB File 
        /// </summary>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        void CreateDb(IHostingEnvironment env);

        /// <summary>
        /// Create the Required Tables for the Project
        /// </summary>
        /// <param name="dbConnection">Object to Connect to the Database</param>
        void CreateTables(IDbConnection dbConnection);

        /// <summary>
        /// Create a new DB Connection
        /// </summary>
        /// <returns>return a new db connection</returns>
        IDbConnection CreateConnection();
    }
}
