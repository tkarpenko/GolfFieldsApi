using Dapper;
using TcpLib;

namespace Golf.Fields.Database
{
    public static partial class DB
    {

        public static async Task<bool> Seed()
        {

            try
            {
                TcpConnection.GetTcpConnectionDetails();

                await CreateDb();

                var result = await TCP.GetSeeds(new TcpGetParams
                {
                    Username = TcpConnection.TcpUsername,
                    Password = TcpConnection.TcpPassword,
                    Server = TcpConnection.TcpHost,
                    Port = TcpConnection.TcpPort,
                });

                var isSuccess = await FetchSqlScriptsAndExecute(result, isSeeding: true);

                return isSuccess;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error during DB seeding: {(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}; {ex.StackTrace}");
            }


            return false;

        }


        private static async Task CreateDb()
        {
            using (var connection = GetConnection(isDbCreating: true))
            {
                var sql = $@"
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{_dbName}')
  BEGIN
    CREATE DATABASE [{_dbName}]
  END";

                connection.Open();

                var res = await connection.ExecuteAsync(sql, commandTimeout: _connectionReTryTimeout);

                connection.Close();
            }

        }
    }
}

