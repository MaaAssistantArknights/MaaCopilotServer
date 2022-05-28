using MaaCopilot.Interfaces.ORM;
using System.Data;
using Dapper;

namespace MaaCopilot.ORM
{
    public class DapperWrapper : IDapperWrapper
    {
        public async Task<int> ExecuteAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? timeout = 0, CommandType? commandType = null)
        {
            return await cnn.ExecuteAsync(sql, param, transaction, timeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? timeout = 0, CommandType? commandType = null)
        {
            return await cnn.QueryAsync<T>(sql, param, transaction, timeout, commandType);
        }
    }
}