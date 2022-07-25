using Golf.Fields.Shared;

namespace Golf.Fields.Database
{
    public static class TcpConnection
    {

        internal static string? TcpHost;

        internal static string? TcpUsername;

        internal static string? TcpPassword;

        internal static int TcpPort;


        public static void GetTcpConnectionDetails()
        {
            if (string.IsNullOrWhiteSpace(TcpHost) ||
                string.IsNullOrWhiteSpace(TcpUsername) ||
                string.IsNullOrWhiteSpace(TcpPassword))
            {
                AppSettingsJson.GetCustomSettings();

                var tcpConnSettings = AppSettingsJson.AppTcpSettings;

                if (tcpConnSettings == null ||
                    string.IsNullOrWhiteSpace(tcpConnSettings.Username) ||
                    string.IsNullOrWhiteSpace(tcpConnSettings.Password) ||
                    tcpConnSettings.Port == 0)
                {
                    throw new TcpException(Resources.Messages.TcpSettingsHasNotBeenProvided);
                }

                TcpHost = Environment.GetEnvironmentVariable("TCP_HOST");

                TcpPort = tcpConnSettings.Port;

                TcpUsername = Encryption.Decrypt(tcpConnSettings.Username);

                TcpPassword = Encryption.Decrypt(tcpConnSettings.Password);

            }
        }
    }
}

