using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MvcDemo.Data
{
    public static class DataBuilder
    {
        public static IDbConnection OpenConnection(IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var conStr = config.GetConnectionString("Db");
            var connection = new SqlConnection(conStr);
            connection.Open();
            return connection;
        }

        public static IDbTransaction BeginTransaction(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            return connection.BeginTransaction();
        }

    }
}
