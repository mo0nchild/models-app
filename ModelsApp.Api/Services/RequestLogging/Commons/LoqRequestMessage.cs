using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModelsApp.Api.Services.RequestLogging.Commons
{
    public sealed class LogRequestMessage : object
    {
        public string MethodName { get; set; } = default!;
        public string UserInfo { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
    }
}
