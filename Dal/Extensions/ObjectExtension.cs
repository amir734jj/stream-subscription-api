using Newtonsoft.Json;
using static Models.Constants.ApplicationConstants;

namespace DAL.Extensions
{
    public static class ObjectExtension
    {
        public static byte[] ToByteArray(this object data)
        {
            var json = JsonConvert.SerializeObject(data);
            
            return DefaultEncoding.GetBytes(json);
        }
    }
}