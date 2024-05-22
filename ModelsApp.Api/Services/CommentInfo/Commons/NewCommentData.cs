using ModelsApp.Api.Commons.Mapping;
using ModelsApp.Dal.Entities;

namespace ModelsApp.Api.Services.CommentInfo.Commons
{
    public class NewCommentData : IMappingTarget<Comment>
    {
        public double Rating { get; set; } = default!;
        public string Text { get; set; } = string.Empty;

        public Guid ModelUuid { get; set; } = Guid.Empty;
        public Guid OwnerUuid { get; set; } = Guid.Empty;
    }
}
