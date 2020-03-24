using Dal.Extensions;
using Models.Utilities;
using Npgsql;
using StackExchange.Redis;

namespace Dal.Utilities
{
    public static class ConnectionStringUtility
    {
        public static string ConnectionStringUrlToRedisResource(string connectionStringUrl)
        {
            var (uri, _) = UrlUtility.UrlToResource(connectionStringUrl);

            var userInfo = uri.UserInfo.Split(':');
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { { uri.Host, uri.Port } },
                ClientName = userInfo[0],
                Password = userInfo[1],
                AbortOnConnectFail = false
            };

            return configurationOptions.ToString();
        }
        
        
        /// <summary>
        /// Converts connection string url to resource
        /// </summary>
        /// <param name="connectionStringUrl"></param>
        /// <returns></returns>
        public static string ConnectionStringUrlToPgResource(string connectionStringUrl)
        {
            var (_, table) = UrlUtility.UrlToResource(connectionStringUrl);

            if (!table.ContainKeys("Host", "Username", "Password", "Database", "ApplicationName"))
            {
                return string.Empty;
            }

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = table["Host"],
                Username = table["Username"],
                Password = table["Password"],
                Database = table["Database"],
                ApplicationName = table["ApplicationName"],
                SslMode = SslMode.Require,
                TrustServerCertificate = true,
                Pooling = true,
                // Hard limit
                MaxPoolSize = 5
            };

            return connectionStringBuilder.ToString();
        }
    }
}