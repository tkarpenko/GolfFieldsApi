using System.Data.SqlClient;
using Golf.Fields.Shared;
using Microsoft.Extensions.Configuration;

namespace Golf.Fields.Database
{
    public static class AppSettingsJson
    {

        internal static AppCustomSettings? AppSettings = null;

        internal static AppDatabaseConnectionSettings? AppDatabaseSettings = null;

        internal static AppTcpConnectionSettings? AppTcpSettings = null;



        public static void GetCustomSettings(string? jsonFileName = null, string? basePath = null)
        {


            if (string.IsNullOrWhiteSpace(jsonFileName))
                jsonFileName = Constants.APP_JSON_NAME;


            if (AppSettings == null)
            {
                IConfigurationBuilder builder;


                if (string.IsNullOrWhiteSpace(basePath))
                {
                    builder = new ConfigurationBuilder().AddJsonFile(jsonFileName, false, true);
                }
                else
                {
                    builder = new ConfigurationBuilder().AddJsonFile(jsonFileName, false, true).SetBasePath(basePath);
                }


                IConfigurationRoot configuration = builder.Build();

                AppSettings = configuration.GetSection(Constants.APP_JSON_CUSTOM_SETTINGS_SECTION).Get<AppCustomSettings>();
            }

            if (AppSettings == null)
            {
                throw new Exception(Resources.Messages.InvalidApplicationCustomSettings);
            }


            AppDatabaseSettings = AppSettings.AppDatabaseConnectionSettings;

            if (AppDatabaseSettings == null)
            {
                throw new Exception(Resources.Messages.DatabaseSettingNotFound);
            }


            AppTcpSettings = AppSettings.AppTcpConnectionSettings;

            if (AppTcpSettings == null)
            {
                throw new Exception(Resources.Messages.TcpConnectionSettingNotFound);
            }

        }



        public static string? GetSqlServerConnString(out string? dbName, out int connectionReTryTimeout, bool isDbCreating = false)
        {

            dbName = null;

            connectionReTryTimeout = 0;


            var connectionString = GetSqlServerConnString(isDbCreating);

            if (AppDatabaseSettings != null)
            {
                dbName = AppDatabaseSettings.Database;

                connectionReTryTimeout = AppDatabaseSettings.ConnectionRetryTimoutSeconds;
            }

            return connectionString;
        }



        public static string? GetSqlServerConnString(bool isDbCreating = false)
        {

            GetCustomSettings();

            if (AppDatabaseSettings == null)
            {
                throw new Exception(Resources.Messages.DatabaseSettingNotFound);
            }


            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            var host = Environment.GetEnvironmentVariable("DB_HOST");

            sqlBuilder.DataSource = AppDatabaseSettings.Port != 0
                ? host + "," + AppDatabaseSettings.Port
                : host;

            if (!isDbCreating)
                sqlBuilder.InitialCatalog = AppDatabaseSettings.Database;

            sqlBuilder.PersistSecurityInfo = true;

            sqlBuilder.UserID = Encryption.Decrypt(AppDatabaseSettings.Username);

            sqlBuilder.Password = Encryption.Decrypt(AppDatabaseSettings.Password);


            if (AppDatabaseSettings.Encrypt)
            {
                sqlBuilder.Encrypt = true;
                sqlBuilder.TrustServerCertificate = true;
            }


            var connectionString = sqlBuilder.ToString();

            return connectionString;
        }

    }
}

