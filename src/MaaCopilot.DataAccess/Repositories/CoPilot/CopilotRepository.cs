using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.Copilot;
using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.Interfaces.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.DataAccess.Repositories.Copilot
{
    public class CopilotRepository : BaseRepository<ORM.Copilot.Copilot>, ICopilotRepository<ORM.Copilot.Copilot>
    {

        public CopilotRepository(IDBProvider dBProvider, IDBHelper dBHelper, IUnitOfWork unitOfWork) : base(dBProvider, dBHelper)
        {
            unitOfWork.Regrister(this);
        }
        public override Task<IEnumerable<ORM.Copilot.Copilot>> Search(SearchDTO request)
        {
            // 
            throw new NotImplementedException();
        }
    }
}
