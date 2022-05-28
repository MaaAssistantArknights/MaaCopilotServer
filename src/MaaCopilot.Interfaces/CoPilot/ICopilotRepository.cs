using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.Copilot
{
    public interface ICopilotRepository<T> : IRepository<T> where T : class
    {
    }
}
