using System.Net;

namespace Models.ViewModels.S3
{
    public class SimpleS3Response
    {
        public HttpStatusCode Status { get; }
        
        public string Message { get; }

        public SimpleS3Response(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}