using AutoMapper;
using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;
using static ModelsApp.Api.Services.ModelInfo.Commons.ModelListData;

namespace ModelsApp.Api.Models.Models.Responses
{
    public class ModelListResponse : IMappingTarget<ModelListData>
    {
        public class ModelItemResponse : IMappingTarget<ModelItemData>
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
        }
        public List<ModelItemResponse> Items { get; set; } = new();
        public int AllCount { get; set; } = default!;
    }
}
