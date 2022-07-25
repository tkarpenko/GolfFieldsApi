using Dapper;
using Dapper.FastCrud;
using Golf.Fields.Shared;

namespace Golf.Fields.Database
{
    public static partial class DB
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string?> GetCurrentVersion()
        {
            string? result = null;

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = $"SELECT [{nameof(DatabaseInfo.Version)}] FROM [{nameof(DatabaseInfo)}]";
                result = await connection.ExecuteScalarAsync<string>(query, null, commandTimeout: _connectionReTryTimeout);

                connection.Close();
            }

            return result;
        }




        public static async Task<DatabaseInfo> GetDatabaseInfo()
        {
            DatabaseInfo info;

            using (var connection = GetConnection())
            {
                string query = $"SELECT * FROM [{nameof(DatabaseInfo)}]";

                connection.Open();

                info = await connection.QueryFirstOrDefaultAsync<DatabaseInfo>(query, null, commandTimeout: _connectionReTryTimeout);

                connection.Close();
            }

            return info;

        }




        private static async Task InsertDatabaseInfo(DatabaseInfo info)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                await connection.InsertAsync(info, statement => statement.WithTimeout(TimeSpan.FromSeconds(_connectionReTryTimeout)));

                connection.Close();
            }
        }



        public static async Task<bool> UpdateDatabaseInfo(DatabaseInfo dbInfo)
        {
            bool success = false;

            using (var connection = GetConnection())
            {
                connection.Open();

                success = await connection.UpdateAsync(dbInfo, statement => statement.WithTimeout(TimeSpan.FromSeconds(_connectionReTryTimeout)));

                connection.Close();
            }

            return success;

        }

    }
}

