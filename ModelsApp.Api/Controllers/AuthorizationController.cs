using ModelsApp.Api.Commons.ConfigureOptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ModelsApp.Api.Models.Authorization.Requests;
using ModelsApp.Api.Models.Authorization.Responses;
using AutoMapper;
using ModelsApp.Api.Services.UserInfo;
using ModelsApp.Api.Services.UserInfo.Commons;
using ModelsApp.Api.Commons.Exceptions;
using Microsoft.Extensions.Logging;

namespace ModelsApp.Api.Controllers
{
    using JwtBearerConfig = ConfigureJwtBearer.JwtBearerConfig;

    [Route("modelsapp/auth"), AllowAnonymous]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private ILogger<AuthorizationController> Logger { get; set; } = default!;

        private readonly JwtBearerConfig tokenConfig = default!;
        private readonly IMapper mapper = default!;
        private readonly IUserInfo userInfoService = default!;
        public AuthorizationController(IOptions<JwtBearerConfig> tokenOptions, IMapper mapper, IUserInfo userInfo) : base()
        {
            this.tokenConfig = tokenOptions.Value;
            this.mapper = mapper;
            this.userInfoService = userInfo;

            this.Logger = LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger<AuthorizationController>();
        }
        /// <summary>
        /// Авторизация пользователя для дальнейшей работы в системе
        /// </summary>
        /// <param name="request">Данные для авторизации пользователя</param>
        /// <returns>Токен авторизации</returns>
        [Route("login"), HttpGet]
        [ProducesResponseType(typeof(AuthorizationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginHandler([FromQuery] LoginRequest request)
        {
            var userInfo = await this.userInfoService.Authorization(request.Login, request.Password);
            if (userInfo == null) return this.Problem("Пользователь не найден");
            var resultClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid, userInfo.Guid.ToString()),
                new Claim(ClaimTypes.Role, "User"),
            };
            var encodingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.tokenConfig.SecretKey));
            var securityToken = new JwtSecurityToken(
                issuer: this.tokenConfig.Issuer,
                audience: this.tokenConfig.Audience,
                claims: resultClaims,
                signingCredentials: new SigningCredentials(encodingKey, SecurityAlgorithms.HmacSha256));

            return this.Ok(new AuthorizationResponse()
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Guid = userInfo.Guid,
            });
        }
        /// <summary>
        /// Регистрация нового пользователя в системе
        /// </summary>
        /// <param name="request">Данные для регистрации пользователя</param>
        /// <returns>Токен авторизации</returns>
        [Route("registration"), HttpPost]
        [ProducesResponseType(typeof(AuthorizationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrationHandler([FromForm] RegistrationRequest request)
        {
            try { await this.userInfoService.AddUser(this.mapper.Map<NewUserData>(request)); }
            catch (ApiException errorInfo)
            {
                this.Logger.LogWarning(errorInfo.Message);
                return this.Problem(errorInfo.Message);
            }
            this.Logger.LogInformation($"Add User: {request.Name}");
            var newRequest = new LoginRequest() { Login = request.Login, Password = request.Password };
            return await this.LoginHandler(newRequest);
        }
    }
}
