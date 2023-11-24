using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Endpoints;

public static class Registration
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