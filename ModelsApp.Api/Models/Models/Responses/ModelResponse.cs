using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Models.Account.Responses;
using ModelsApp.Api.Services.ModelInfo.Commons;
using ModelsApp.Api.Services.UserInfo.Commons;

namespace ModelsApp.Api.Models.Models.Responses
{
    public class ModelResponse : IMappingTarget<ModelData>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;

        public int Downloads { get; set; } = default;
        public int Views { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public double Rating { get; set; } = default!;

        public string CategoryName { get; set; } = string.Empty;
        public string? ImageName { get; set; } = default;

        public AccountResponse Owner { get; set; } = default!;
        public ModelFileInfo Info { get; set; } = default!;
    }
}
