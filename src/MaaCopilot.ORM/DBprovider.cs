using MaaCopilot.Interfaces.ORM;
using System.Data;
using Dapper;
using MaaCopilot.DataTransferObjects;
using System.Data.Common;

namespace MaaCopilot.ORM
{
    public class DBProvider : IDBProvider
    {
        protected bool _disposed = false;
        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection;
        protected readonly IDapperWrapper _dbWrapper;
        protected readonly IDBHelper _dbHelper;
        public DBProvider(IDbConnection dbConnection, IDapperWrapper dapperWrapper, IDBHelper dBHelper)
        {
            _dbConnection = dbConnection;
            _dbWrapper = dapperWrapper;
            _dbHelper = dBHelper;
        }

        public void CloseConnection()
        {
            if (_dbConnection.State != ConnectionState.Closed) _dbConnection.Close();
        }

        public async Task<int> DelegteAsync<T>(T data)
        {
            return await UpdateAsync(data, new List<string>().ToArray());
        }

        public async Task OpenConnectionAsync()
        {
            if (_dbConnection == null || _dbConnection.State == ConnectionState.Closed)
            {
                await ((DbConnection)_dbConnection).OpenAsync().ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<T>> Search<T>(SearchDTO reqeust)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync<T>(T data, string[] ignoreColumns)
        {
            var q = $"Insert Into {typeof(T).Name} ({_dbHelper.GenerateColumns<T>(ignoreColumns)}) values ({_dbHelper.GenerateParams<T>(ignoreColumns)}); SELECT last_insert_id();  ";
            var res = await _dbWrapper.QueryAsync<int>(_dbConnection, q, data, _dbTransaction);
            return res.Single();
        }

        public async Task<int> UpdateAsync<T>(T data, string[] ignoreColumns)
        {
            var q = $"Update {typeof(T).Name} Set ({_dbHelper.GenerateColumns<T>(ignoreColumns, true)}) where {_dbHelper.GenerateParams<T>(ignoreColumns, true)} And Enabled = True; ";
            var res = await _dbWrapper.ExecuteAsync(_dbConnection, q, data, _dbTransaction);
            return res;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Rollback();
                    _dbTransaction.Dispose();
                }
                if (_dbConnection.State != ConnectionState.Closed)
                {
                    _dbConnection.Close();                    
                }
                _dbConnection.Dispose();
                _dbConnection = null;
                _disposed = true;
                _dbTransaction = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}