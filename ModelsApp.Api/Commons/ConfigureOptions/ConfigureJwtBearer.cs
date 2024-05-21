using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ModelsApp.Api.Commons.ConfigureOptions
{
    using JwtBearerConfig = ConfigureJwtBearer.JwtBearerConfig;
    public partial class ConfigureJwtBearer(IOptions<JwtBearerConfig> options) : IConfigureNamedOptions<JwtBearerOptions>
    {
        protected JwtBearerConfig Configuration { get; private set; } = options.Value;
        protected byte[] SecurityKey { get => Encoding.UTF8.GetBytes(this.Configuration.SecretKey); }

        public sealed class JwtBearerConfig : object
        {
            public string Issuer { get; set; } = default!;
            public string Audience { get; set; } = default!;
            public string SecretKey { get; set; } = default!;
        }
        public virtual void Configure(string? name, JwtBearerOptions options) => this.Configure(options);
        public virtual void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = this.Configuration.Issuer,
                ValidateIssuer = true,

                ValidAudience = this.Configuration.Audience,
                ValidateAudience = true,

                IssuerSigningKey = new SymmetricSecurityKey(this.SecurityKey),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = false,
            };
            options.SaveToken = true;
        }
    }
}
