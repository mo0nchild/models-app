namespace ModelsApp.Api.Services.S3Storage.Infrastructure
{
    public class BucketInfo : object
    {
        public string BucketName { get; set; } = string.Empty;
        public string ObjectName { get; set; } = string.Empty;
    }
}
