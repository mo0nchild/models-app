namespace ModelsApp.Api.Services.ModelInfo.Commons
{
    public enum SortingType : sbyte { ByDate, ByViewing, ByRating }
    public class GetModelList : object
    {
        public int Skip { get; set; } = default!;
        public int Take { get; set; } = default!;

        public SortingType SortingType { get; set; } = default!;

        public string? Category { get; set; } = default!;
        public string? NameFilter { get; set; } = default!;
    }
}
