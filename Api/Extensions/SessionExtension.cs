using Microsoft.AspNetCore.Http;
using Models.Constants;

namespace API.Extensions
{
    public static class SessionExtension
    {
        /// <summary>
        /// Extension method to quickly get the username/password
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static (string username, string password) GetUseramePassword(this ISession session) =>
            (session.GetString(ApiConstants.Username), session.GetString(ApiConstants.Password));

        /// <summary>
        /// Extension method to check whether user is logged in or not
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool IsAuthenticated(this ISession session) => session.GetString(ApiConstants.Authenticated.Key) == ApiConstants.Authenticated.Value;
    }
}