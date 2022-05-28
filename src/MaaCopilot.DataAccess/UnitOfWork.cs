using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.Interfaces.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDBProvider iDBProvider;
        private bool disposedValue = false;
        public Dictionary<Type, dynamic> Repo { get; private set; }
        public UnitOfWork(IDBProvider dBProvider)
        {
            iDBProvider = dBProvider;
            Repo = new Dictionary<Type, dynamic>();
        }
        public void CloseConnection()
        {
            iDBProvider.CloseConnection();
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                iDBProvider.Dispose();
                iDBProvider = null;
            }
            disposedValue = true;
        }

        public Task OpenConnectionAsync()
        {
            return iDBProvider.OpenConnectionAsync();
        }

        public void Regrister<T>(IRepository<T> repository) where T : class
        {
            dynamic repo;
            if (!Repo.TryGetValue(typeof(T), out repo))
            {
                Repo.Add(typeof(T), repo);
            }
        }

        public dynamic Repositoory<T>() where T : class
        {
            dynamic repository;
            if (Repo.TryGetValue(typeof(T), out repository))
            {
                return repository;
            }
            return null;
        }
    }
}
