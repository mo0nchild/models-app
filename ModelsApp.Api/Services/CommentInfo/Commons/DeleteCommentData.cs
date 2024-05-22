namespace ModelsApp.Api.Services.CommentInfo.Commons
{
    public class DeleteCommentData : object
    {
        public Guid OwnerUuid { get; set; } = Guid.Empty;
        public Guid ModelUuid { get; set; } = Guid.Empty;
    }
}
