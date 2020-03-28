using System.Collections.Generic;
using System.Net;

namespace Models.ViewModels.S3
{
    public class DownloadS3Response : SimpleS3Response
    {
        public byte[] Data { get; }
        
        public IReadOnlyDictionary<string, string> MetaData { get; }
        
        public string ContentType { get; }
        
        public string Name { get; }

        public DownloadS3Response(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public DownloadS3Response(HttpStatusCode status, string message, byte[] data, IReadOnlyDictionary<string, string> metaData, string contentType, string name) : base(status, message)
        {
            Data = data;
            MetaData = metaData;
            ContentType = contentType;
            Name = name;
        }
    }
}