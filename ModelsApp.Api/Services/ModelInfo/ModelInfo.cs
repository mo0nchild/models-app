using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Services.ModelInfo.Commons;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Api.Services.S3Storage.Infrastructure;
using ModelsApp.Api.Services.UserInfo.Commons;
using ModelsApp.Dal;
using ModelsApp.Dal.Entities;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ModelsApp.Api.Services.ModelInfo
{
    public class ModelInfo : IModelInfo
    {
        private readonly static string ImageBucketName = "images", ModelBucketName = "models";
        private readonly static int ExpiryAccess = 60;

        private readonly IDbContextFactory<ModelsDbContext> contextFactory = default!;
        private readonly IMapper mapper = default!;
        private readonly IS3StorageService storageService = default!;
        public ModelInfo(IDbContextFactory<ModelsDbContext> contextFactory, IMapper mapper,
            IS3StorageService storageService) : base()
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.storageService = storageService;
        }
        public async Task AddModel(NewModelData modelData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var category = await dbContext.ModelCategories.FirstOrDefaultAsync(item => item.Name == modelData.CategoryName);
                if (category == null) throw new ApiException("Категория не найдена", typeof(ModelInfo));

                var owner = await dbContext.UserProfiles.FirstOrDefaultAsync(item => item.Guid == modelData.OwnerUuid);
                if (owner == null) throw new ApiException("Пользователь не найден", typeof(ModelInfo));

                var mappedData = this.mapper.Map<Model>(modelData);
                mappedData.Guid = Guid.NewGuid();
                mappedData.DateTime = DateTime.UtcNow;

                mappedData.CategoryId = category.Id;
                mappedData.OwnerId = owner.Id;
                mappedData.ImageName = $"{Guid.NewGuid()}.{modelData.Image.FileName.Split('.')[1]}"; ;
                var imageStorageInfo = new BucketInfo()
                {
                    BucketName = ModelInfo.ImageBucketName,
                    ObjectName = mappedData.ImageName
                };
                var imageLoaded = await this.storageService.LoadObjectToStorage(modelData.Image, imageStorageInfo);
                if (!imageLoaded) throw new ApiException("Изображение не удалось загрузить", typeof(ModelInfo));

                mappedData.Info.Filename = $"{Guid.NewGuid()}.{modelData.File.FileName.Split('.')[1]}";
                var modelStorageInfo = new BucketInfo()
                {
                    BucketName = ModelInfo.ModelBucketName,
                    ObjectName = mappedData.Info.Filename
                };
                if (!(await this.storageService.LoadObjectToStorage(modelData.Image, modelStorageInfo)))
                {
                    await this.storageService.RemoveObjectFromStorage(imageStorageInfo);
                    throw new ApiException("Модель не удалось загрузить", typeof(ModelInfo));
                }
                await dbContext.Models.AddAsync(mappedData);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateModel(UpdateModelData modelData)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Models.FirstOrDefaultAsync(item => item.Guid == modelData.UUID);
                if (record == null) throw new ApiException("Не удалось найти модель", typeof(ModelInfo));

                var category = await dbContext.ModelCategories.FirstOrDefaultAsync(item => item.Name == modelData.CategoryName);
                if (category == null) throw new ApiException("Категория не найдена", typeof(ModelInfo));
            
                record.CategoryId = category.Id;
                record.Name = modelData.Name;
                record.Description = modelData.Description;

                await dbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteModel(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Models.Where(item => item.Guid == uuid)
                    .Include(item => item.Info).FirstOrDefaultAsync();
                if (record == null) throw new ApiException("Не Удалось найти модель", typeof(ModelInfo));

                await this.storageService.RemoveObjectFromStorage(new BucketInfo()
                {
                    BucketName = ModelInfo.ImageBucketName,
                    ObjectName = record.ImageName!
                });
                await this.storageService.RemoveObjectFromStorage(new BucketInfo()
                {
                    BucketName = ModelInfo.ModelBucketName,
                    ObjectName = record.Info.Filename
                });
                dbContext.ModelInfos.RemoveRange(record.Info);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<byte[]?> GetDataByUUID(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Models.Where(item => item.Guid == uuid)
                    .Include(item => item.Info).FirstOrDefaultAsync();
                if (record == null) return null;
                record.Downloads++;
                await dbContext.SaveChangesAsync();
                return await this.storageService.GetObjectFromStorage(new BucketInfo()
                {
                    BucketName = ModelInfo.ModelBucketName,
                    ObjectName = record.Info.Filename
                });
            }
        }
        public async Task<ModelData?> GetInfoByUUID(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Models.Where(item => item.Guid == uuid)
                    .Include(item => item.Info)
                    .Include(item => item.Owner)
                    .Include(item => item.Comments)
                    .Include(item => item.Category).FirstOrDefaultAsync();
                if(record == null) return null;
                var mappedRecord = this.mapper.Map<ModelData>(record);
                mappedRecord.ImageName = await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
                {
                    BucketName = ModelInfo.ImageBucketName,
                    ObjectName = record.ImageName!
                }, ModelInfo.ExpiryAccess);
                record.Views++;
                await dbContext.SaveChangesAsync();
                return mappedRecord;
            }
        }
        public async Task<ModelListData> GetInfoList(GetModelList request)
        {
            var sortedRating = (Model item) => item.Comments.Sum(op => (double)op.Rating / item.Comments.Count());
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var records = await dbContext.Models.Include(item => item.Category)
                    .Where(item => request.Category == null ? true : request.Category == item.Category.Name)
                    .Where(item => request.NameFilter == null ? true :
                        Regex.IsMatch(item.Name, request.NameFilter, RegexOptions.IgnoreCase))
                    .Include(item => item.Comments).ToListAsync();
                var orderedResult = (request.SortingType switch
                {
                    SortingType.ByRating => records.OrderByDescending(item => sortedRating(item)),
                    SortingType.ByDate => records.OrderByDescending(item => item.DateTime),
                    SortingType.ByViewing => records.OrderByDescending(item => item.Views),
                    _ => throw new ApiException("Не установлен режим сортировки", typeof(ModelInfo))
                });
                foreach (var item in orderedResult)
                {
                    item.ImageName = await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
                    {
                        BucketName = ModelInfo.ImageBucketName,
                        ObjectName = item.ImageName!
                    }, ModelInfo.ExpiryAccess);
                }
                var filteredRecord = orderedResult.Skip(request.Skip).Take(request.Take).ToList()!;
                return new ModelListData()
                {
                    Items = this.mapper.Map<List<ModelItemData>>(filteredRecord),
                    AllCount = orderedResult.Count(),
                };
            }
        }
        public async Task<ModelListData> GetOwnedList(Guid uuid)
        {
            using (var dbContext = await this.contextFactory.CreateDbContextAsync())
            {
                var record = await dbContext.Models.Include(item => item.Category)
                    .Include(item => item.Owner)
                    .Where(item => item.Owner.Guid == uuid).ToListAsync();
                foreach (var item in record)
                {
                    item.ImageName = await this.storageService.GetObjectUrlFromStorage(new BucketInfo()
                    {
                        BucketName = ModelInfo.ImageBucketName,
                        ObjectName = item.ImageName!
                    }, ModelInfo.ExpiryAccess);
                }
                return new ModelListData()
                {
                    Items = this.mapper.Map<List<ModelItemData>>(record),
                    AllCount = record.Count()
                };
            }
        }
    }
}
