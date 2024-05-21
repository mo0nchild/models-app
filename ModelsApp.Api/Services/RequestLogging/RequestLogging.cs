using Microsoft.EntityFrameworkCore;
using ModelsApp.Api.Services.RequestLogging.Commons;

namespace ModelsApp.Api.Services.RequestLogging
{
    public partial class RequestLogging : IRequestLogging
    {
        protected ILogger<RequestLogging> Logger { get; private set; } = default!;
        public RequestLogging(ILogger<RequestLogging> logger) : base() => this.Logger = logger;
        public virtual async Task LogRequest(LogRequestMessage message)
        {
            this.Logger.LogInformation($"Access Info: [ UserIP: {message.UserInfo}; Date/Time: {message.DateTime}; " +
                $"MethodName: {message.MethodName} ]");
        }
    }
}
