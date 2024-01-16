using FastEndpoints.Swagger;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Middlewares;

namespace Presentation.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApp(this IApplicationBuilder builder)
    {
        builder
            .UseMiddleware<GlobalExceptionHandlingMiddleware>()
            .UseEndpoints()
            .UseSwaggerGen();

        return (WebApplication)builder;
    }
}