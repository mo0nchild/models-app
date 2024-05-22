using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Models.Bookmarks.Responses;
using ModelsApp.Api.Models.Comments.Requests;
using ModelsApp.Api.Models.Comments.Responses;
using ModelsApp.Api.Services.BookmarkInfo;
using ModelsApp.Api.Services.CommentInfo;
using ModelsApp.Api.Services.CommentInfo.Commons;
using ModelsApp.Api.Services.ModelInfo.Commons;
using ModelsApp.Api.Services.UserInfo.Commons;
using System.Net;
using System.Security.Claims;

namespace ModelsApp.Api.Controllers
{
    [Route("modelsapp/comments"), Authorize]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ILogger<AuthorizationController> Logger { get; set; } = default!;

        private readonly IMapper mapper = default!;
        private readonly ICommentInfo commentInfoService = default!;
        public CommentsController(IMapper mapper, ICommentInfo commentInfo) : base()
        {
            this.Logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<AuthorizationController>();
            (this.commentInfoService, this.mapper) = (commentInfo, mapper);
        }
        [Route("add"), HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCommentHandler([FromForm] AddCommentRequest request)
        {
            var userUuid = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!);
            var mappedRequest = this.mapper.Map<NewCommentData>(request);
            mappedRequest.OwnerUuid = userUuid;
            try { await this.commentInfoService.AddComment(mappedRequest); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Add Comment: {request.ModelUuid}");
            return this.Ok("Комментарий успешно добавлен");
        }
        [Route("delete"), HttpDelete]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteCommentHandler([FromQuery] Guid modelUuid)
        {
            var deleteRequest = new DeleteCommentData()
            {
                OwnerUuid = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!),
                ModelUuid = modelUuid,
            };
            try { await this.commentInfoService.DeleteComment(deleteRequest); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Remove Comment: {modelUuid}");
            return this.Ok("Комментарий успешно удален");
        }
        [Route("getList"), HttpGet]
        [ProducesResponseType(typeof(CommentsListResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCommentsHandler([FromQuery] Guid modelUuid)
        {
            try {
                var bookmarks = await this.commentInfoService.GetCommentsList(modelUuid);
                return this.Ok(this.mapper.Map<CommentsListResponse>(bookmarks));
            }
            catch (ApiException errorInfo) { return this.BadRequest(errorInfo.Message); }
        }
    }
}
