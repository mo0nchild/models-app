using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Models.Authorization.Requests;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using ModelsApp.Api.Services.UserInfo.Commons;
using ModelsApp.Dal;
using ModelsApp.Dal.Entities;
using System.Data.Common;
using System.Threading;

namespace ModelsApp.Api.Services.UserInfo
{
    using BCryptType = BCrypt.Net.BCrypt;
    public class UserInfo : IUserInfo
    {
        private readonly static string BucketName = "images";
        private readonly static int ExpiryAccess = 60;

        private readonly IDbContextFactory<ModelsDbContext> contextFactory = default!;
        private readonly IMapper mapper = default!;
        private readonly IS3StorageService storageService = default!;
        public UserInfo(IDbContextFactory<ModelsDbContext> contextFactory, IMapper mapper,
            IS3StorageService storageService) : base() 
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.storageService = storageService;
        }
        public async Task<UserData?> Authorization(string login, string password)
        {
            var verifyPassword = (string hashPassword) =>
            {
                try { return BCryptType.Verify(password, hashPassword, false, BCrypt.Net.HashType.SHA384); }
                catch (BCrypt.Net.SaltParseException) { return false; }
            };
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var profiles = await dbContext.Authorizations.Where(item => item.Login == login)
                   .Include(item => item.UserProfile).ToListAsync();

                var result = profiles.FirstOrDefault(item => item.Login == login && verifyPassword(item.Password));
                return result == null ? null : this.mapper.Map<UserData>(result.UserProfile);
            }
        }
        protected virtual async Task UploadUserImage(IFormFile image, string imageName)
        {
            var imageLoaded = await this.storageService.LoadObjectToStorage(image, new BucketInfo()
            {
                BucketName = UserInfo.BucketName,
                ObjectName = imageName
            });
            if (!imageLoaded) throw new ApiException("Изображение не удалось загрузить", typeof(UserInfo));
        }
        public async Task AddUser(NewUserData userData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var checking = await dbContext.UserProfiles.Include(item => item.Authorization)
                    .FirstOrDefaultAsync(item => item.Authorization.Login == userData.Login && item.Email == userData.Email);
                if (checking != null)
                {
                    throw new ApiException("Email или логин уже используется", typeof(UserInfo));
                }
                var mappedRecord = this.mapper.Map<UserProfile>(userData);
                mappedRecord.Guid = Guid.NewGuid();
                mappedRecord.DateTime = DateTime.UtcNow;
                if (userData.Image != null)
                {
                    mappedRecord.ImageName = $"{Guid.NewGuid()}.{userData.Image.FileName.Split('.')[1]}";
                    await this.UploadUserImage(userData.Image, mappedRecord.ImageName);
                }
                await dbContext.UserProfiles.AddAsync(mappedRecord);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateUser(UpdateUserData userData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.UserProfiles.FirstOrDefaultAsync(item => item.Guid == userData.UUID);
                if (record == null)
                {
                    throw new ApiException("Пользователь не найден", typeof(UserInfo));
                }
                record.Name = userData.Name;
                record.Biography = userData.Biography;
                if (userData.Image != null)
                {
                    if (record.ImageName != null)
                    {
                        var imageRemoving = await this.storageService.RemoveObjectFromStorage(new BucketInfo()
                        {
                            BucketName = UserInfo.BucketName,
                            ObjectName = record.ImageName
                        });
                        if (!imageRemoving) throw new ApiException("Не удалось обработать изображение", typeof(UserInfo));

                    }
                    else record.ImageName = $"{Guid.NewGuid()}.{userData.Image.FileName.Split('.')[1]}";
                    await this.UploadUserImage(userData.Image, record.ImageName);
                }
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteUser(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.UserProfiles.FirstOrDefaultAsync(item => item.Guid == uuid);
                if (record == null) throw new ApiException("Не удалось найти пользователя", typeof(UserInfo));
                if(record.ImageName != null)
                {
                    var removingImage = await this.storageService.RemoveObjectFromStorage(new BucketInfo()
                    {
                        BucketName = UserInfo.BucketName,
                        ObjectName = record.ImageName
                    });
                    if (!removingImage) throw new ApiException("Не удалось получить доступ к изображению", typeof(UserInfo));
                }
                dbContext.UserProfiles.Remove(record);
                await dbContext.SaveChangesAsync();
            }
        }
        protected virtual async Task<string?> GetImageForProfile(string imageName)
        {
            return await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
            {
                BucketName = UserInfo.BucketName,
                ObjectName = imageName
            }, UserInfo.ExpiryAccess);
        }
        public async Task<UserData?> GetByEmail(string email)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = dbContext.UserProfiles.FirstOrDefault(item => item.Email == email);
                if (record == null) return null;
                var mappedRecord = this.mapper.Map<UserData>(record);

                if (mappedRecord.ImageName == null) return mappedRecord;
                mappedRecord.ImageName = await this.GetImageForProfile(mappedRecord.ImageName);

                return mappedRecord;
            }
        }
        public async Task<UserData?> GetByUUID(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = dbContext.UserProfiles.FirstOrDefault(item => item.Guid == uuid);
                if (record == null) return null;
                var mappedRecord = this.mapper.Map<UserData>(record);

                if (mappedRecord.ImageName == null) return mappedRecord;
                mappedRecord.ImageName = await this.GetImageForProfile(mappedRecord.ImageName);

                return mappedRecord;
            }
        }
    }
}
