using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Models.Utilities
{
    public static class UrlUtility
    {
        /// <summary>
        /// Converts connection string url to resource
        /// </summary>
        /// <param name="connectionStringUrl"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> UrlToResource(string connectionStringUrl)
        {
            var isUrl = Uri.TryCreate(connectionStringUrl, UriKind.Absolute, out var url);

            if (!isUrl)
            {
                return ImmutableDictionary.Create<string, string>();
            }

            var connectionStringBuilder = new Dictionary<string, string>
            {
                ["Host"] = url.Host,
                ["Username"] = url.UserInfo.Split(':')[0],
                ["Password"] = url.UserInfo.Split(':')[1],
                ["Database"] = url.LocalPath.Substring(1),
                ["ApplicationName"] = "milwaukee-internationals"
            };

            return connectionStringBuilder;
        }
    }
}