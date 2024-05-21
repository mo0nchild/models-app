using ModelsApp.Api.Services.RequestLogging;
using ModelsApp.Api.Services.RequestLogging.Commons;
using System.Net;

namespace ModelsApp.Api.Commons.Middlewares
{
    public partial class RequestLoggingMiddleware : object
    {
        protected internal readonly RequestDelegate requestDelegate = default!;
        protected internal readonly IRequestLogging requestLogging = default!;

        public RequestLoggingMiddleware(RequestDelegate requestDelegate, IRequestLogging requestLogging) : base()
        {
            this.requestDelegate = requestDelegate;
            this.requestLogging = requestLogging;
        }
        public virtual async Task InvokeAsync(HttpContext context)
        {
            await this.requestLogging.LogRequest(new LogRequestMessage()
            {
                UserInfo = (context.Connection.RemoteIpAddress ?? IPAddress.Loopback).ToString(),
                DateTime = DateTime.UtcNow,
                MethodName = context.Request.Path,
            });
            await this.requestDelegate.Invoke(context);
        }
    }
    public static class UseRequestLoggingMiddlewareExtension : object
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
