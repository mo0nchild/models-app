using Microsoft.Extensions.Options;
using static ModelsApp.Api.Commons.ConfigureOptions.ConfigureApiAccess;

namespace ModelsApp.Api.Commons.ConfigureOptions
{
    public partial class ConfigureApiAccess : IConfigureNamedOptions<ApiAccessOptions>
    {
        protected internal readonly IConfiguration configuration = default!;
        public ConfigureApiAccess(IConfiguration configuration) : base() => this.configuration = configuration;

        public sealed class ApiAccessOptions : object
        {
            public required string ApiKey { get; set; } = default!;
        }

        public virtual void Configure(string? name, ApiAccessOptions options) => this.Configure(options);

        public virtual void Configure(ApiAccessOptions options)
        {
            if (this.configuration["ApiKey"] == null) throw new Exception("Не установлен ключ API");
            options.ApiKey = this.configuration["ApiKey"]!;
        }
    }
}
