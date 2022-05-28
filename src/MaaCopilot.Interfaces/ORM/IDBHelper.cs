using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.ORM
{
    public interface IDBHelper  
    {
        string GenerateParams<T>( string[] ignoreColumns, bool update = false);
        string GenerateColumns<T>(string[] ignoreColumns, bool update = false);
    }
}
