using System.Collections.Concurrent;
using Logic.Interfaces;

namespace Logic.DataStructures
{
    public class IdentityDictionary : IIdentityDictionary
    {
        public ConcurrentDictionary<string, string> AuthenticatedUsers { get; }

        public IdentityDictionary()
        {
            AuthenticatedUsers = new ConcurrentDictionary<string, string>();
        }
    }
}