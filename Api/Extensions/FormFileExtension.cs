using System.IO;
using System.Threading.Tasks;
using Logic.Extensions;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class FormFileExtension
    {
        public static async Task<MemoryStream> ToMemoryStream(this IFormFile formFile)
        {
            // Short-circuit
            if (formFile == null)
            {
                return new MemoryStream();
            }
            
            await using var data = new MemoryStream();
            await formFile.CopyToAsync(data);

            return data.Reset();
        }
    }
}