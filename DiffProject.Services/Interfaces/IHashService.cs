using System;
using System.Collections.Generic;
using System.Text;

namespace DiffProject.Services.Interfaces
{
    public interface IHashService
    {
        string CreateHash(string input);
    }
}
