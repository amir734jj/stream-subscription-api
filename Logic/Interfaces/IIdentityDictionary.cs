using System.Collections.Concurrent;

namespace Logic.Interfaces
{
    public interface IIdentityDictionary
    {
        ConcurrentDictionary<string, string> AuthenticatedUsers { get; }
    }
}