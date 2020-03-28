using System;
using System.Collections.Generic;
using System.Net;

namespace Models.ViewModels.S3
{
    public class UriS3Response : SimpleS3Response
    {
        public Uri Uri { get; }

        public IReadOnlyDictionary<string, string> MetaData { get; }
        
        public string ContentType { get; }
        
        public string Name { get; }
        
        public UriS3Response(HttpStatusCode status, string message) : base(status, message) { }

        public UriS3Response(HttpStatusCode status, string message, Uri uri, IReadOnlyDictionary<string, string> metaData, string contentType, string name) : base(status, message)
        {
            Uri = uri;
            MetaData = metaData;
            ContentType = contentType;
            Name = name;
        }
    }
}