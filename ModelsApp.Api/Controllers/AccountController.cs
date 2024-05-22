using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Models.Account.Requests;
using ModelsApp.Api.Models.Account.Responses;
using ModelsApp.Api.Models.Authorization.Responses;
using ModelsApp.Api.Services.UserInfo;
using ModelsApp.Api.Services.UserInfo.Commons;
using System.Net;
using System.Security.Claims;
using static ModelsApp.Api.Commons.ConfigureOptions.ConfigureJwtBearer;

namespace ModelsApp.Api.Controllers
{
    [Route("modelsapp/profile"), Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILogger<AuthorizationController> Logger { get; set; } = default!;

        private readonly IMapper mapper = default!;
        private readonly IUserInfo userInfoService = default!;
        public AccountController(IMapper mapper, IUserInfo userInfo) : base()
        {
            this.Logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<AuthorizationController>();
            (this.userInfoService, this.mapper) = (userInfo, mapper);
        }
        [Route("getInfo"), HttpGet]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetInfoHandler()
        {
            var userUuid = this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
            if (userUuid == null) return this.BadRequest("Нельзя прочитать UUID");

            var userData = await this.userInfoService.GetByUUID(Guid.Parse(userUuid));
            if (userData == null) return this.BadRequest("Пользователь не найден");

            return this.Ok(this.mapper.Map<AccountResponse>(userData));
        }
        [Route("update"), HttpPut]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateInfoHandler([FromForm] UpdateAccountRequest request)
        {
            var userUuid = this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
            if (userUuid == null) return this.Problem("Нельзя прочитать UUID");

            var mappedRequest = this.mapper.Map<UpdateUserData>(request);
            mappedRequest.UUID = Guid.Parse(userUuid);
            try { await this.userInfoService.UpdateUser(mappedRequest); }
            catch(ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Update User: {mappedRequest.UUID}");
            return this.Ok("Данные пользователя обновлены");
        }
        [Route("delete"), HttpDelete]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteInfoHandler()
        {
            var userUuid = this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
            if (userUuid == null) return this.Problem("Нельзя прочитать UUID");

            try { await this.userInfoService.DeleteUser(Guid.Parse(userUuid)); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.BadRequest(errorInfo.Message);
            }
            this.Logger.LogInformation($"Remove User: {userUuid}");
            return this.Ok("Пользователь успешно удален");
        }
    }
}
