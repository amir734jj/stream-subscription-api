using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        public static async Task<Stream> ConvertToBase64(this Stream stream)
        {
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
                
            var base64 = Convert.ToBase64String(bytes);
            return new MemoryStream(Encoding.UTF8.GetBytes(base64));
        }
    }
}