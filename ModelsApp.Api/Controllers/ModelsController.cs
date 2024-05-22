using AutoMapper;
using CommunityToolkit.HighPerformance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using ModelsApp.Api.Commons.Exceptions;
using ModelsApp.Api.Models.Account.Responses;
using ModelsApp.Api.Models.Models.Requests;
using ModelsApp.Api.Models.Models.Responses;
using ModelsApp.Api.Services.ModelInfo;
using ModelsApp.Api.Services.ModelInfo.Commons;
using System.Net;
using System.Security.Claims;

namespace ModelsApp.Api.Controllers
{
    [Route("modelsapp/models"), Authorize]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private ILogger<AuthorizationController> Logger { get; set; } = default!;

        private readonly IMapper mapper = default!;
        private readonly IModelInfo modelInfoService = default!;
        public ModelsController(IMapper mapper, IModelInfo userInfo) : base()
        {
            this.Logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<AuthorizationController>();
            (this.modelInfoService, this.mapper) = (userInfo, mapper);
        }
        protected virtual async Task<bool> CheckProfileAccess(Guid resourceUuid)
        {
            var profileUuid = this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid);
            if (profileUuid == null) throw new Exception("Нельзя прочитать UUID");

            var ownedList = await this.modelInfoService.GetOwnedList(Guid.Parse(profileUuid));
            return ownedList.Items.FirstOrDefault(item => item.Guid == resourceUuid) != null;
        }
        [Route("add"), HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddModelHandler([FromForm] AddModelRequest request)
        {
            var mappedRequest = this.mapper.Map<NewModelData>(request);
            mappedRequest.OwnerUuid = Guid.Parse(this.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!);
            try { await this.modelInfoService.AddModel(mappedRequest); }
            catch (ApiException errorInfo) 
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.Problem(errorInfo.Message);
            }
            this.Logger.LogInformation($"Add Model: {mappedRequest.Name}");
            return this.Ok("Модель успешно добавлена");
        }
        [Route("update"), HttpPut]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateModelHandler([FromForm] UpdateModelRequest request)
        {
            if (!(await this.CheckProfileAccess(request.UUID)))
            {
                return this.Problem("Модель не принадлежит пользователю");
            }
            var mappedRequest = this.mapper.Map<UpdateModelData>(request);
            try { await this.modelInfoService.UpdateModel(mappedRequest); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.Problem(errorInfo.Message);
            }
            this.Logger.LogInformation($"Update Model: {mappedRequest.Name}");
            return this.Ok("Модель успешно обновлена");
        }
        [Route("delete"), HttpDelete]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteModelHandler([FromQuery] Guid modelUuid)
        {
            if (!(await this.CheckProfileAccess(modelUuid)))
            {
                return this.Problem("Модель не принадлежит пользователю");
            }
            try { await this.modelInfoService.DeleteModel(modelUuid); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.Problem(errorInfo.Message);
            }
            this.Logger.LogInformation($"Remove Model: {modelUuid}");
            return this.Ok("Модель успешно удалена");
        }
        [Route("getInfo"), HttpGet]
        [ProducesResponseType(typeof(ModelResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetInfoHandler([FromQuery] Guid uuid)
        {
            var result = await this.modelInfoService.GetInfoByUUID(uuid);
            if (result == null) return this.Problem("Модель не найдена");

            return this.Ok(this.mapper.Map<ModelResponse>(result));
        }
        [Route("getList"), HttpGet]
        [ProducesResponseType(typeof(ModelListResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetListHandler([FromQuery] GetModelsListRequest request)
        {
            var result = await this.modelInfoService.GetInfoList(this.mapper.Map<GetModelList>(request));
            return this.Ok(this.mapper.Map<ModelListResponse>(result));
        }
        [Route("getOwnerList"), HttpGet]
        [ProducesResponseType(typeof(ModelListResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOwnerListHandler([FromQuery] Guid ownerUuid)
        {
            var result = await this.modelInfoService.GetOwnedList(ownerUuid);
            return this.Ok(this.mapper.Map<ModelListResponse>(result));
        }
    }
}
