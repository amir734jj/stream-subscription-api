namespace Dal.Configs
{
    public class S3ServiceConfig
    {
        public string BucketName { get; }
        
        public string Prefix { get; }

        public S3ServiceConfig(string bucketName, string prefix)
        {
            BucketName = bucketName;
            Prefix = prefix;
        }
    }
}