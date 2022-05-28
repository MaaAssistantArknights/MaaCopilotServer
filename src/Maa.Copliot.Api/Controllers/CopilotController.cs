using MaaCopilot.DataTransferObjects.Copilot;
using MaaCopilot.Service.Interfaces.Copilot;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CopilotController : BaseController<CopilotDTO, ORM.Copilot.Copilot, UpdateCopilotDTO>
    {
        private readonly ICopilotService _copilotService;
        public CopilotController(ICopilotService service) : base(service)
        {
            _copilotService = service; 
        }
    }
}