using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Extensions
{
    public static class LinqExtension
    {
        public static async IAsyncEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, Task<bool>> filter)
        {
            foreach (var item in source)
            {
                if (await filter(item))
                {
                    yield return item;
                }
            }
        }
    }
}