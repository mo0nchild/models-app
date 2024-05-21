
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using System.Xml.Linq;

namespace ModelsApp.Api.Services.S3Storage
{
    public class S3StorageService : IS3StorageService
    {
        private readonly IMinioClientFactory minioFactory = default!;
        private readonly ILogger<S3StorageService> logger = default!;
        public S3StorageService(IMinioClientFactory minioFactory, ILogger<S3StorageService> logger) : base()
        {
            (this.minioFactory, this.logger) = (minioFactory, logger);
        }
        public async Task<byte[]?> GetObjectFromStorage(BucketInfo info)
        {
            using (var minioClient = this.minioFactory.CreateClient())
            {
                using var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs().WithBucket(info.BucketName).WithObject(info.ObjectName)
                                     .WithCallbackStream((stream) =>
                                     {
                                         stream.CopyTo(memoryStream);
                                     });
                var result = await minioClient.GetObjectAsync(getObjectArgs);
                return result == null ? null : memoryStream.ToArray();
            }
        }
        public async Task<string?> GetObjectUrlFromStorage(BucketInfo info, int expiry)
        {
            using (var minioClient = this.minioFactory.CreateClient())
            {
                var args = new PresignedGetObjectArgs().WithBucket(info.BucketName)
                                .WithObject(info.ObjectName)
                                .WithExpiry(expiry);
                var presignedUrl = await minioClient.PresignedGetObjectAsync(args);

                this.logger.LogInformation($"Object presigned URL: ${presignedUrl}");
                return presignedUrl;
            }
        }
        public async Task<bool> LoadObjectToStorage(IFormFile file, BucketInfo info)
        {
            using (var minioClient = this.minioFactory.CreateClient())
            {
                var checkBucketArgs = new BucketExistsArgs().WithBucket(info.BucketName);
                if (!await minioClient.BucketExistsAsync(checkBucketArgs)) return false;
                using (var filestream = file.OpenReadStream())
                {
                    using var requestData = new MemoryStream();
                    filestream.CopyTo(requestData);
                    requestData.Seek(0, SeekOrigin.Begin);

                    

                    var putObjectArgs = new PutObjectArgs().WithBucket(info.BucketName)
                        .WithStreamData(requestData)
                        .WithContentType("application/octet-stream")
                        .WithObject($"{info.ObjectName}")
                        .WithObjectSize(file.Length);

                    try { await minioClient.PutObjectAsync(putObjectArgs); }
                    catch (MinioException errorInfo) { this.logger.LogError(errorInfo.Message); return false; }
                }
                this.logger.LogInformation($"Successfully uploaded to storage: {info.ObjectName}");
                return true;
            }
        }
        public async Task<bool> RemoveObjectFromStorage(BucketInfo info)
        {
            using (var minioClient = this.minioFactory.CreateClient())
            {
                var args = new RemoveObjectArgs().WithBucket(info.BucketName)
                                .WithObject(info.ObjectName);
                try { await minioClient.RemoveObjectAsync(args); }
                catch(MinioException errorInfo) { this.logger.LogError(errorInfo.Message); return false; }
                
                this.logger.LogInformation($"Successfully removed from storage: {info.ObjectName}");
                return true;
            }
        }
    }
}
