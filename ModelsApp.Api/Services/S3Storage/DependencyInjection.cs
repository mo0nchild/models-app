using Minio;
using Minio.DataModel.Args;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using System.Net;
using System.Runtime.CompilerServices;

namespace ModelsApp.Api.Services.S3Storage
{
    public static class DependencyInjection : object
    {
        public async static Task<IServiceCollection> AddS3Storage(this IServiceCollection collection, IConfiguration configuration)
        {
            var options = configuration.GetSection("MinioStorage").Get<S3StorageOptions>();
            if (options == null) throw new Exception("Not Found S3Storage options");
            collection.AddMinio(storageOptions =>
            {
                storageOptions.WithEndpoint(options.S3Endpoint, options.S3Port);
                storageOptions.WithProxy(new WebProxy(options.S3ProxyEndpoint, options.S3Port));

                storageOptions.WithSSL(false);
                storageOptions.WithCredentials(options.S3AccessKey, options.S3SecretKey);
            });
            var minioFactory = collection.BuildServiceProvider().GetService<IMinioClientFactory>()!;
            using (var minioClient = minioFactory.CreateClient())
            {
                if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("models")))
                    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("models"));

                if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("images")))
                    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("images"));
            }
            collection.AddTransient<IS3StorageService, S3StorageService>();
            return collection;
        }
    }
}
