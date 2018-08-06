using System;
using System.Collections.Generic;

namespace Models.Constants
{
    public class ApiConstants
    {
        public const string AuthenticationSessionCookieName = "AuthenticationCookie";
        
        /// <summary>
        /// Authenticated token
        /// </summary>
        public static readonly KeyValuePair<string, string> Authenticated = new KeyValuePair<string, string>("Authenticated", Guid.NewGuid().ToString());
        
        public const string Username = "Username";

        public const string Password = "Password";
    }
}