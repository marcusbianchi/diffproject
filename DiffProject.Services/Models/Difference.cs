using System;
using System.Collections.Generic;
using System.Text;

namespace DiffProject.Services.Models
{
    /// <summary>
    /// Class to hold the differences between string
    /// </summary>
    public class Difference
    {
        /// <summary>
        /// Start position of the difference
        /// </summary>
        public int DifferenceStartPosition{ get; set; }

        /// <summary>
        /// Size of the Difference
        /// </summary>
        public int DifferenceSize { get; set; }
    }
}
