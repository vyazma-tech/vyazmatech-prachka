using FastEndpoints.Swagger;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Middlewares;
using Serilog;

namespace Presentation.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApp(this IApplicationBuilder builder)
    {
        builder
            .UseSerilogRequestLogging()
            .UseMiddleware<RequestLogContextMiddleware>()
            .UseMiddleware<GlobalExceptionHandlingMiddleware>()
            .UseEndpoints()
            .UseSwaggerGen();

        return (WebApplication)builder;
    }
}