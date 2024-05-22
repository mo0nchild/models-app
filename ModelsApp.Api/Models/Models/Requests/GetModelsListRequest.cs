using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.ModelInfo.Commons;

namespace ModelsApp.Api.Models.Models.Requests
{
    public class GetModelsListRequest : IMappingTarget<GetModelList>
    {
        public int Skip { get; set; } = default!;
        public int Take { get; set; } = default!;

        public SortingType SortingType { get; set; } = default!;

        public string? Category { get; set; } = default!;
        public string? NameFilter { get; set; } = default!;
    }
}
