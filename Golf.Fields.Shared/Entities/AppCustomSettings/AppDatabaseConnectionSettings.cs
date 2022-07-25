using System;
namespace Golf.Fields.Shared
{
    public class AppDatabaseConnectionSettings
    {
        public int Port { get; set; }

        public string? Server { get; set; }

        public string? Database { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool Encrypt { get; set; }

        public int ConnectionRetryTimoutSeconds { get; set; } = Constants.DATABASE_DEFAULT_CONNECTION_RETRY_SECONDS;
    }
}

