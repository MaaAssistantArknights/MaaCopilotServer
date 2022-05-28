using MaaCopilot.DataTransferObjects;

namespace MaaCopilot.Interfaces.ORM
{
    public interface IDBProvider : IDisposable
    {
        Task OpenConnectionAsync();
        void CloseConnection();
        Task<int> AddAsync<T>(T data, string[] ignoreColumns);
        Task<IEnumerable<T>> Search<T>(SearchDTO reqeust);
        Task<int> DelegteAsync<T>(T data);
        Task<int> UpdateAsync<T>(T data, string[] ignoreColumns);

    }
}
