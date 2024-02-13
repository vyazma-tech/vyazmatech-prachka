using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        return services.AddFastEndpoints();
    }

    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        return app.UseFastEndpoints();
    }
}