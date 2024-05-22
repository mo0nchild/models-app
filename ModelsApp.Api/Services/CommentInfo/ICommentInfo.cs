using ModelsApp.Api.Services.CommentInfo.Commons;

namespace ModelsApp.Api.Services.CommentInfo
{
    public interface ICommentInfo
    {
        public Task AddComment(NewCommentData commentData);
        public Task DeleteComment(DeleteCommentData commentData);

        public Task<CommentListData> GetCommentsList(Guid modelUuid);
    }
}
