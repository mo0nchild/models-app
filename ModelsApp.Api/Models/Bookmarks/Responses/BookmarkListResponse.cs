using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.BookmarkInfo.Commons;

namespace ModelsApp.Api.Models.Bookmarks.Responses
{
    public class BookmarkItemResponse : IMappingTarget<BookmarkItemData>
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
    public class BookmarkListResponse : IMappingTarget<BookmarkListData>
    {
        public List<BookmarkItemResponse> Items { get; set; } = new();
        public int AllCount { get; set; } = default!;
    }
}
