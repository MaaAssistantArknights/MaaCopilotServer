using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.DataAccess.Repositories.Copilot
{
    public class CopilotRepository : BaseRepository<ORM.Copilot.Copilot>
    {
        protected readonly IDBProvider _dbProvider;

        public CopilotRepository(IDBProvider dBProvider, IDBHelper dBHelper) : base(dBProvider, dBHelper)
        {
            _dbProvider = dBProvider;
        }
        public override Task<IEnumerable<ORM.Copilot.Copilot>> Search(SearchDTO request)
        {
            // 
            throw new NotImplementedException();
        }
    }
}
