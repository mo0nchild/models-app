using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.BookmarkInfo.Commons;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using ModelsApp.Dal;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.BookmarkInfo
{
    public class BookmarkInfo : IBookmarkInfo
    {
        private readonly static string ImageBucketName = "images";
        private readonly static int ExpiryAccess = 60;

        private readonly IDbContextFactory<ModelsDbContext> contextFactory = default!;
        private readonly IMapper mapper = default!;
        private readonly IS3StorageService storageService = default!;
        public BookmarkInfo(IDbContextFactory<ModelsDbContext> contextFactory, IMapper mapper,
            IS3StorageService storageService) : base()
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.storageService = storageService;
        }
        public async Task AddBookmark(BookmarkConnection bookmarkData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var modelRecord = await dbContext.Models.FirstOrDefaultAsync(item => item.Guid == bookmarkData.ModelUUID);
                if (modelRecord == null) throw new ApiException("Модель не найдена", typeof(BookmarkInfo));

                var userRecord = await dbContext.UserProfiles.FirstOrDefaultAsync(item => item.Guid == bookmarkData.UserUUID);
                if (userRecord == null) throw new ApiException("Пользователь не найден", typeof(BookmarkInfo));

                var checkingExists = await dbContext.Bookmarks.Include(item => item.User)
                    .Include(item => item.Model)
                    .FirstOrDefaultAsync(item => item.User.Guid == bookmarkData.UserUUID && item.Model.Guid == bookmarkData.ModelUUID);
                if (checkingExists != null) throw new ApiException("Заметка уже добавлена", typeof(BookmarkInfo));

                await dbContext.Bookmarks.AddRangeAsync(new Bookmark()
                {
                    DateTime = DateTime.UtcNow,
                    UserId = userRecord.Id,
                    ModelId = modelRecord.Id,
                });
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteBookmark(BookmarkConnection bookmarkConnection)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Bookmarks.Include(item => item.User)
                    .Include(item => item.Model)
                    .Where(item => item.Model.Guid == bookmarkConnection.ModelUUID && item.User.Guid == bookmarkConnection.UserUUID)
                    .FirstOrDefaultAsync();
                if (record == null) throw new ApiException("Заметка не найдена", typeof(BookmarkInfo));
                dbContext.Bookmarks.RemoveRange(record);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<BookmarkListData> GetBookmarksList(Guid userUuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var modelRecords = await dbContext.Bookmarks.Include(item => item.User)
                    .Include(item => item.Model).ThenInclude(item => item.Category)
                    .Include(item => item.Model).ThenInclude(item => item.Comments)
                    .Where(item => item.User.Guid == userUuid)
                    .Select(item => item.Model).ToListAsync();
                foreach (var item in modelRecords)
                {
                    item.ImageName = await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
                    {
                        BucketName = BookmarkInfo.ImageBucketName,
                        ObjectName = item.ImageName!
                    }, BookmarkInfo.ExpiryAccess);
                }
                return new BookmarkListData()
                {
                    Items = this.mapper.Map<List<BookmarkItemData>>(modelRecords),
                    AllCount = modelRecords.Count(),
                };
            }
        }
    }
}
