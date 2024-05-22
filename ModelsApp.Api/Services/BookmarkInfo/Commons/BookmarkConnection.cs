namespace ModelsApp.Api.Services.BookmarkInfo.Commons
{
    public class BookmarkConnection : object
    {
        public Guid UserUUID { get; set; } = Guid.Empty;
        public Guid ModelUUID { get; set; } = Guid.Empty;
    }
}
