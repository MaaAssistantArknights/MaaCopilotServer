using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.Service
{
    public  interface IServiceResponse<T>
    {
        bool Success { get; }
        string Message { get; } 
        T ResponseObject { get; }           
    }
}
