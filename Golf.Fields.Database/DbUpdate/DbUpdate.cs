using Dapper;
using TcpLib;

namespace Golf.Fields.Database
{
    public static partial class DB
    {


        public static async Task<bool> CheckNewerVersions(string currentVersion)
        {
            TcpConnection.GetTcpConnectionDetails();

            var result = await TCP.GetVersionsFrom(new TcpGetParams
            {
                Username = TcpConnection.TcpUsername,
                Password = TcpConnection.TcpPassword,
                Server = TcpConnection.TcpHost,
                Port = TcpConnection.TcpPort,
                CurrentVersion = currentVersion
            });

            var isSuccess = await FetchSqlScriptsAndExecute(result);

            return isSuccess;
        }



        private static async Task UpgradeDb(string? sql)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                await connection.ExecuteAsync(sql, commandTimeout: _connectionReTryTimeout);

                connection.Close();
            }
        }



        private static async Task<bool> FetchSqlScriptsAndExecute(TcpGetResult<string[]> tcpGetResult, bool isSeeding = false)
        {

            if (!tcpGetResult.IsSuccess)
            {
                return false;
            }

            if (tcpGetResult.Result == null)
            {
                return true;
            }

            if (tcpGetResult.Result.Any())
            {
                foreach (var sqlRelativePath in tcpGetResult.Result)
                {
                    var getFileResult = await TCP.GetFile(new TcpGetParams
                    {
                        Username = TcpConnection.TcpUsername,
                        Password = TcpConnection.TcpPassword,
                        Server = TcpConnection.TcpHost,
                        Port = TcpConnection.TcpPort,
                        RelativeFilePath = sqlRelativePath
                    });

                    if (!getFileResult.IsSuccess)
                    {
                        return false;
                    }

                    else if (getFileResult.Result != null)
                    {

                        var sqlVersion = Path.GetFileNameWithoutExtension(sqlRelativePath);

                        var isSuccess = await ExecuteSql(getFileResult.Result, sqlVersion, isSeeding);

                        if (!isSuccess)
                            return false;
                    }
                }

                return true;
            }

            return false;
        }




        private static async Task<bool> ExecuteSql(string sql, string version, bool isSeeding)
        {
            try
            {
                await UpgradeDb(sql);

                var message = isSeeding
                    ? Resources.Messages.SuccessfullySeededBatch
                    : Resources.Messages.SuccessfullyUpdatedToVersion;

                Console.WriteLine(message.Replace("{0}", version));

            }
            catch (Exception ex)
            {

                if (!isSeeding)
                {
                    var dbInfo = await GetDatabaseInfo();

                    string error = $"{Resources.Messages.FailedUpgradeToVersion.Replace("{0}", version)}: {(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}";

                    Console.WriteLine(error);

                    dbInfo.ModifiedOn = DateTime.UtcNow;
                    dbInfo.LastError = error;

                    await UpdateDatabaseInfo(dbInfo);
                }

                return false;
            }

            return true;
        }

    }
}

