using System;
namespace Golf.Fields.Shared
{
    public class AppCustomSettings
    {
        public AppDatabaseConnectionSettings? AppDatabaseConnectionSettings { get; set; }

        public AppTcpConnectionSettings? AppTcpConnectionSettings { get; set; }
    }
}

