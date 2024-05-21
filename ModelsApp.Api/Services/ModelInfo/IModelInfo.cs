using ModelsApp.Api.Services.ModelInfo.Commons;

namespace ModelsApp.Api.Services.ModelInfo
{
    public interface IModelInfo
    {
        public Task AddModel(NewModelData modelData);
        public Task UpdateModel(UpdateModelData modelData);
        public Task DeleteModel(Guid uuid);

        public Task<ModelListData> GetInfoList(int skip, int take);
        public Task<ModelListData> GetOwnedList(Guid uuid);

        public Task<ModelData?> GetInfoByUUID(Guid uuid);
        public Task<byte[]?> GetDataByUUID(Guid uuid);
    }
}
