using Serilog.Context;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Middlewares;

internal class RequestLogContextMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            return next(context);
        }
    }
}