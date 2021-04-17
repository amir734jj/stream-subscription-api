using Newtonsoft.Json;
using static Models.Constants.ApplicationConstants;

namespace Dal.Extensions
{
    public static class ObjectExtension
    {
        public static byte[] ObjectToByteArray<T>(this T data)
        {
            var json = JsonConvert.SerializeObject(data);
            
            return DefaultEncoding.GetBytes(json);
        }
    }
}