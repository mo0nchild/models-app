using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Api.Services.CommentInfo.Commons;

namespace ModelsApp.Api.Models.Comments.Responses
{
    public class CommentOwnerResponse : IMappingTarget<CommentOwnerData>
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? Biography { get; set; } = default!;
        public string? ImageName { get; set; } = default!;
    }
    public class CommentItemResponse : IMappingTarget<CommentItemData>
    {
        public Guid Guid { get; set; } = Guid.Empty;

        public double Rating { get; set; } = default!;
        public string Text { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = default!;

        public CommentOwnerResponse User { get; set; } = default!;
    }
    public class CommentsListResponse : IMappingTarget<CommentListData>
    {
        public List<CommentItemResponse> Items { get; set; } = new();
        public int AllCount { get; set; } = default!;
    }
}
