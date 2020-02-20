using Dal.Extensions;
using Models.Models;
using Models.Utilities;
using Npgsql;
using StackExchange.Redis;

namespace Api.Utilities
{
    public static class ConnectionStringUtility
    {
        public static string ConnectionStringUrlToRedisResource(string connectionStringUrl)
        {
            var (uri, table) = UrlUtility.UrlToResource(connectionStringUrl);

            if (!table.ContainKeys("Host", "Username", "Password", "Database"))
            {
                return string.Empty;
            }
            
            ConfigurationOptions.Parse($"{table["Username"]}:{table["Password"]},{table["Host"]},defaultDatabase={table["Database"]}");

            return string.Empty;
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