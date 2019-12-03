using System.Data;

namespace Starter.Data.Connections
{
    public interface IConnection
    {
        IDbConnection Create();

        IDbCommand CreateSpCommand(string sql, IDbDataParameter[] paramArray);
    }
}