using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.Interfaces.ORM;

namespace MaaCopilot.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDBProvider _dBProvider;
        protected readonly IDBHelper _dBHelper;
        protected BaseRepository(IDBProvider dBProvider,IDBHelper dBHelper )
        {
            _dBHelper = dBHelper;
            _dBProvider = dBProvider;
        }

        public async Task<int> AddAsync(T data, string[] ignoreColumns)
        {
            return await _dBProvider.AddAsync(data, ignoreColumns);
        }

        public abstract Task<IEnumerable<T>> Search(SearchDTO request);


        public async Task<int> UpdateAsync(T data, string[] ignoreColumns)
        {
            return await _dBProvider.UpdateAsync(data, ignoreColumns);
        }

    }
}
