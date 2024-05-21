namespace ModelsApp.Api.Services.S3Storage.Infrastructure
{
    public class S3StorageOptions : object
    {
        public string S3AccessKey { get; set; } = default!;
        public string S3SecretKey { get; set; } = default!;

        public string S3Endpoint { get; set; } = default!;
        public string S3ProxyEndpoint { get; set; } = default!;
        public int S3Port { get; set; } = default!;
    }
}
