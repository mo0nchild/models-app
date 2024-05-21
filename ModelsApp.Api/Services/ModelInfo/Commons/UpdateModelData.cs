using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.ModelInfo.Commons
{
    public class UpdateModelData : IMappingTarget<Model>
    {
        public Guid UUID { get; set; } = Guid.Empty;

        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
