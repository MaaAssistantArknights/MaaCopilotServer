using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaCopilot.DataTransferObjects.Copilot;
using MaaCopilot.Interfaces.Services;

namespace MaaCopilot.Service.Interfaces.Copilot
{
    public interface ICopilotService : IService<CopilotDTO, ORM.Copilot.Copilot>
    {

    }
}
