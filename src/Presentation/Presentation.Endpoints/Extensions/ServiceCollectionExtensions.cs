using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        return services.AddFastEndpoints();
    }

    public static IApplicationBuilder UseEndpoints(this WebApplication app)
    {
        return app.UseFastEndpoints();
    }
}