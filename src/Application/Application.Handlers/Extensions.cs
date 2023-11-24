using Microsoft.Extensions.DependencyInjection;

namespace Application.Handlers;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediator();
    }
}