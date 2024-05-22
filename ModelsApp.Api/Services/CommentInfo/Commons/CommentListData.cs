using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.CommentInfo.Commons
{
    public class CommentOwnerData : IMappingTarget<UserProfile>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? Biography { get; set; } = default!;
        public string? ImageName { get; set; } = default!;
    }
    public class CommentItemData : IMappingTarget<Comment>
    {
        public Guid Guid { get; set; } = Guid.Empty;

        public double Rating { get; set; } = default!;
        public string Text { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = default!;
        public CommentOwnerData User { get; set; } = default!;
    }
    public class CommentListData : object
    {
        public List<CommentItemData> Items { get; set; } = new();
        public int AllCount { get; set; } = default!;
    }
}
