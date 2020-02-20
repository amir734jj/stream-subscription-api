using System.Collections.Generic;
using System.Linq;

namespace Dal.Extensions
{
    public static class DictionaryExtension
    {
        public static bool ContainKeys<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, params TKey[] keys)
        {
            return !keys.Intersect(source.Keys).Except(keys).Any();
        }
    }
}