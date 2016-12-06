using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Dms.Repository
{
    public static class DbStorages
    {
        public static DbContext C2CSearchEngineReaderConnection = null;
        public static DbContext C2CSearchEngineWriterConnection = null;
        public static DbContext LotteryWriterConnection = null;

        static DbStorages() {
            LotteryWriterConnection = new DbContext(Dependencies.LotteryWriterConnection);
        }
    }

    public class DbContext
    {
        string connectionString = string.Empty;
        public DbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Execute(CommandDefinition command)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.Execute(command);
            }
        }
        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        }
        public IEnumerable<T> Query<T>(CommandDefinition command)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.Query<T>(command);
            }
        }
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }
        public T QuerySingle<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                return conn.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }
    }
}
