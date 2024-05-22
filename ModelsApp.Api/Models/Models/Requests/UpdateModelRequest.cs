using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;

namespace ModelsApp.Api.Models.Models.Requests
{
    public class UpdateModelRequest : IMappingTarget<UpdateModelData>
    {
        public Guid UUID { get; set; } = Guid.Empty;

        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
