using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services
            .AddFastEndpoints();

        if (environment.IsProduction() is false)
        {
            services
                .SwaggerDocument(
                    options =>
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

        return services;
    }

    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseFastEndpoints(
            config =>
            {
                config.Versioning.Prefix = "v";
                config.Versioning.PrependToRoute = true;
                config.Endpoints.RoutePrefix = "api";
                config.Versioning.DefaultVersion = 1;
            });

        if (environment.IsProduction() is false)
            app.UseSwaggerGen();

        return app;
    }
}