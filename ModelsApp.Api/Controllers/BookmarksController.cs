using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Models.Bookmarks.Responses;
using ModelsApp.Api.Models.Models.Requests;
using ModelsApp.Api.Services.BookmarkInfo;
using ModelsApp.Api.Services.BookmarkInfo.Commons;
using ModelsApp.Api.Services.ModelInfo;
using ModelsApp.Api.Services.ModelInfo.Commons;
using System.Net;
using System.Security.Claims;

namespace ModelsApp.Api.Controllers
{
    [Route("modelsapp/bookmarks"), Authorize]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private ILogger<AuthorizationController> Logger { get; set; } = default!;

        private readonly IMapper mapper = default!;
        private readonly IBookmarkInfo bookmarkInfoService = default!;
        public BookmarksController(IMapper mapper, IBookmarkInfo bookmarkInfo) : base()
        {
            this.Logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<AuthorizationController>();
            (this.bookmarkInfoService, this.mapper) = (bookmarkInfo, mapper);
        }
        [Route("add"), HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddBookmarkHandler([FromForm] Guid modelUuid)
        {
            var serviceRequest = new BookmarkConnection() 
            {
                UserUUID = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!),
                ModelUUID = modelUuid,
            };
            try { await this.bookmarkInfoService.AddBookmark(serviceRequest); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Add Bookmark: {modelUuid}");
            return this.Ok("Заметка успешно добавлена");
        }
        [Route("delete"), HttpDelete]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBookmarkHandler([FromQuery] Guid modelUuid)
        {
            var serviceRequest = new BookmarkConnection()
            {
                UserUUID = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!),
                ModelUUID = modelUuid,
            };
            try { await this.bookmarkInfoService.DeleteBookmark(serviceRequest); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Remove Bookmark: {modelUuid}");
            return this.Ok("Заметка успешно удалена");
        }
        [Route("getList"), HttpGet]
        [ProducesResponseType(typeof(BookmarkListResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookmarksHandler()
        {
            var userUuid = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!);
            var bookmarks = await this.bookmarkInfoService.GetBookmarksList(userUuid);

            return this.Ok(this.mapper.Map<BookmarkListResponse>(bookmarks));
        }
    }
}
