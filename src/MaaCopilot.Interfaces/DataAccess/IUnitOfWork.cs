
namespace MaaCopilot.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
        Dictionary<Type, dynamic> Repo { get; }
        Task OpenConnectionAsync();
        void CloseConnection();
        dynamic Repositoory<T>() where T : class;
        void Regrister<T>(IRepository<T> repository) where T : class;
    }
}
