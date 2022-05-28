using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.Services
{
    public interface IService<U,T>
        where U : class
        where T : class
    {
        Task<IServiceResponse<U>> AddAsync(U data);
        Task<IServiceResponse<U>> Search(SearchDTO request);
        Task<IServiceResponse<U>> UpdateAsync(U data);
    }
}
