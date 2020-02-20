using System;
using Microsoft.AspNetCore.Routing;

namespace Api.Extensions
{
    public static class RouteDataExtension
    {
        /// <summary>
        /// Returns the controller from route data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetController(this RouteData data)
        {
            return data.Values["Controller"].ToString();
        }

        /// <summary>
        /// Returns the controller action
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetAction(this RouteData data)
        {
            return data.Values["action"].ToString();
        }

        /// <summary>
        /// Returns matches flag
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Matches(this RouteData data, string controller, string action = null)
        {
            return string.Equals(data.GetController(), controller, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(data.GetAction(), action ?? data.GetAction(),
                       StringComparison.CurrentCultureIgnoreCase);
        }
    }
}