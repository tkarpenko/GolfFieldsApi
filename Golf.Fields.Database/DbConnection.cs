using System.Data.SqlClient;
using Golf.Fields.Shared;

namespace Golf.Fields.Database
{

    public static partial class DB
    {

        private static int _connectionReTryTimeout = 0;

        private static string? _dbName;



        private static SqlConnection GetConnection(bool isDbCreating = false)
        {
            string? connectionString;

            if (string.IsNullOrWhiteSpace(_dbName))
            {
                connectionString = AppSettingsJson.GetSqlServerConnString(out _dbName, out _connectionReTryTimeout, isDbCreating);
            }
            else
            {
                connectionString = AppSettingsJson.GetSqlServerConnString(isDbCreating);
            }

            Console.WriteLine($"DB connection str: {connectionString}");

            var connection = new SqlConnection(connectionString);

            if (connection == null)
            {
                throw new DatabaseException(Resources.Messages.DbConnectionCouldNotBeEstablished);
            }

            return connection;

        }

    }
}

