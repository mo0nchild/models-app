using ModelsApp.Api.Services.S3Storage.Infrastructure;

namespace ModelsApp.Api.Services.S3Storage
{
    public interface IS3StorageService
    {
        public Task<string?> GetObjectUrlFromStorage(BucketInfo info, int expiry);
        public Task<byte[]?> GetObjectFromStorage(BucketInfo info);

        public Task<bool> LoadObjectToStorage(IFormFile file, BucketInfo info);
        public Task<bool> RemoveObjectFromStorage(BucketInfo info);
    }
}
