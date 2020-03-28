using System.IO;
using Newtonsoft.Json;
using static Models.Constants.ApplicationConstants;

namespace Dal.Extensions
{
    public static class ByteArrayExtension
    {
        public static T Deserialize<T>(this byte[] data) where T : class
        {
            using var stream = new MemoryStream(data);
            using var reader = new StreamReader(stream, DefaultEncoding);
            return JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
        }
    }
}