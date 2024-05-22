using ModelsApp.Api.Services.BookmarkInfo.Commons;

namespace ModelsApp.Api.Services.BookmarkInfo
{
    public interface IBookmarkInfo
    {
        public Task AddBookmark(BookmarkConnection bookmarkData);
        public Task DeleteBookmark(BookmarkConnection bookmarkConnection);

        public Task<BookmarkListData> GetBookmarksList(Guid userUuid);
    }
}
