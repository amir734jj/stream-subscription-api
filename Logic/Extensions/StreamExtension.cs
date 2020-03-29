using System;
using System.IO;

namespace Logic.Extensions
{
    public static class StreamExtension
    {
        public static MemoryStream Reset(this MemoryStream stream)
        {
            // Needed to reset the stream
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static string ConvertToBase64(this MemoryStream stream)
        {
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}