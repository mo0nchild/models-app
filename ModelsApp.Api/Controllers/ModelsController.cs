using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using ModelsApp.Api.Models.Account.Responses;
using ModelsApp.Api.Models.Models.Responses;
using ModelsApp.Api.Services.ModelInfo;
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
        [ProducesResponseType(typeof(ModelResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetInfoHandler([FromQuery] int skip, [FromQuery] int take)
        {
            var result = await this.modelInfoService.GetInfoList(skip, take);
            return this.Ok(this.mapper.Map<ModelListResponse>(result));
        }

    }
}
