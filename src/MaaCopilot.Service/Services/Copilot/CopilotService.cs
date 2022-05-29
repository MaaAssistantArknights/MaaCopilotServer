using log4net;
using MaaCopilot.DataTransferObjects.Copilot;
using MaaCopilot.Interfaces.Copilot;
using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.Interfaces.ORM;
using MaaCopilot.Service.Interfaces.Copilot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Service.Services.Copilot
{
    public class CopilotService : Service<CopilotDTO, ORM.Copilot.Copilot>, ICopilotService
    {
        //protected readonly ILog _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IDBHelper _dBHelper;
        protected readonly ICopilotRepository<ORM.Copilot.Copilot> _copilotRepository;

        public CopilotService(IDBHelper dBHelper, IUnitOfWork unitOfWork, ICopilotRepository<ORM.Copilot.Copilot> copilotRepository) : base(dBHelper, unitOfWork)
        {
            _dBHelper = dBHelper;               
            _unitOfWork = unitOfWork;
            _copilotRepository = copilotRepository;
            //_logger = logger;
        }
    }
}
