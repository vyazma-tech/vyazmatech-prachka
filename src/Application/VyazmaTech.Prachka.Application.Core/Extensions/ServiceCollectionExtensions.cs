using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Core.Users;

namespace VyazmaTech.Prachka.Application.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCurrentUsers(this IServiceCollection services)
    {
        services.AddScoped<CurrentUserProxy>();
        services.AddScoped<ICurrentUser>(x => x.GetRequiredService<CurrentUserProxy>());
        services.AddScoped<ICurrentUserManager>(x => x.GetRequiredService<CurrentUserProxy>());

        return services;
    }
}