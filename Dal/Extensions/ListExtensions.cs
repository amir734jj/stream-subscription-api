using System;
using System.Collections.Generic;

namespace Dal.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random Random = new Random();  

        public static List<T> Shuffle<T>(this List<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }

            return list;
        }
    }
}