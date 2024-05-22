using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Services.CommentInfo.Commons;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using ModelsApp.Dal;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.CommentInfo
{
    public class CommentInfo : ICommentInfo
    {
        private readonly static string ImageBucketName = "images";
        private readonly static int ExpiryAccess = 60;

        private readonly IDbContextFactory<ModelsDbContext> contextFactory = default!;
        private readonly IMapper mapper = default!;
        private readonly IS3StorageService storageService = default!;
        public CommentInfo(IDbContextFactory<ModelsDbContext> contextFactory, IMapper mapper,
            IS3StorageService storageService) : base()
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.storageService = storageService;
        }
        public async Task AddComment(NewCommentData commentData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var modelRecord = await dbContext.Models.FirstOrDefaultAsync(item => item.Guid == commentData.ModelUuid);
                if (modelRecord == null) throw new ApiException("Модель не найдена", typeof(CommentInfo));

                var userRecord = await dbContext.UserProfiles.FirstOrDefaultAsync(item => item.Guid == commentData.OwnerUuid);
                if (userRecord == null) throw new ApiException("Пользователь не найдена", typeof(CommentInfo));

                var record = await dbContext.Comments.Include(item => item.User)
                    .Include(item => item.Model)
                    .FirstOrDefaultAsync(item => item.User.Guid == commentData.OwnerUuid && item.Model.Guid == commentData.ModelUuid);
                if (record == null)
                {
                    var mappedRecord = this.mapper.Map<Comment>(commentData);
                    mappedRecord.UserId = userRecord.Id;
                    mappedRecord.ModelId = modelRecord.Id;

                    mappedRecord.DateTime = DateTime.UtcNow;
                    mappedRecord.Guid = Guid.NewGuid();
                    await dbContext.Comments.AddAsync(mappedRecord);
                }
                else
                {
                    record.Rating = commentData.Rating;
                    record.DateTime = DateTime.UtcNow;
                    record.Text = commentData.Text;
                }
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteComment(DeleteCommentData commentData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Comments.Include(item => item.User)
                    .Include(item => item.Model)
                    .Where(item => item.Model.Guid == commentData.ModelUuid && item.User.Guid == commentData.OwnerUuid)
                    .FirstOrDefaultAsync();
                if (record == null) throw new ApiException("Комментарий не найден", typeof(CommentInfo));
                dbContext.Comments.RemoveRange(record);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<CommentListData> GetCommentsList(Guid modelUuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var modelRecord = await dbContext.Models.Where(item => item.Guid == modelUuid)
                    .Include(item => item.Comments).ThenInclude(item => item.User)
                    .FirstOrDefaultAsync();
                if (modelRecord == null) throw new ApiException("Модель не найдена", typeof(CommentInfo));
                foreach (var item in modelRecord.Comments)
                {
                    if (item.User.ImageName == null) continue;
                    item.User.ImageName = await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
                    {
                        BucketName = CommentInfo.ImageBucketName,
                        ObjectName = item.User.ImageName!
                    }, CommentInfo.ExpiryAccess);
                }
                return new CommentListData()
                {
                    Items = this.mapper.Map<List<CommentItemData>>(modelRecord.Comments),
                    AllCount = modelRecord.Comments.Count()
                };
            }
        }
    }
}
