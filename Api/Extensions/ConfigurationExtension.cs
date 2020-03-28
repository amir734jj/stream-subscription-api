using System;
using Microsoft.Extensions.Configuration;

namespace Api.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Gets required or throws an exception
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRequiredValue<T>(this IConfiguration configuration, string key) where T: class
        {
            return configuration.GetValue<T>(key) ?? throw new Exception($"Failed to get configuration for: {key} and type: {typeof(T).Name}");
        }
    }
}