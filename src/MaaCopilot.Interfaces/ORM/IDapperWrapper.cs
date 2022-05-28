using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Interfaces.ORM
{
    public interface IDapperWrapper       
    {
        Task<int> ExecuteAsync(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? timeout = 0, CommandType? commandType = default(CommandType?));
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? timeout = 0, CommandType? commandType = default(CommandType?));
    }
}
