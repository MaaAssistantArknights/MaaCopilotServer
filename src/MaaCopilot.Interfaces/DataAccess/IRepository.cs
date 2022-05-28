using MaaCopilot.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Search(SearchDTO request);
        Task<int> AddAsync(T data, string[] ignoreColumns);
        Task<int> UpdateAsync(T data, string[] ignoreColumns);
    }
}
