using ModelsApp.Api.Services.RequestLogging.Commons;

namespace ModelsApp.Api.Services.RequestLogging
{
    public interface IRequestLogging
    {
        public Task LogRequest(LogRequestMessage message);
    }
}
