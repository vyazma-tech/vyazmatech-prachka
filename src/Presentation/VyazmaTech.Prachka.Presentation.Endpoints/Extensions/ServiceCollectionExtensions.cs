using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        return services
            .AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                options.MaxEndpointVersion = 1;
                options.DocumentSettings = s =>
                {
                    s.DocumentName = "v1";
                    s.Title = "API";
                    s.Version = "v1";
                };
            });
    }

    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        return app.UseFastEndpoints(config =>
        {
            config.Versioning.Prefix = "v";
            config.Versioning.PrependToRoute = true;
            config.Endpoints.RoutePrefix = "api";
            config.Versioning.DefaultVersion = 1;
        });
    }
}