using Microsoft.AspNetCore.Authorization;
using VyazmaTech.Prachka.Presentation.Authorization.Contracts;
using VyazmaTech.Prachka.Presentation.Authorization.Models;
using VyazmaTech.Prachka.Presentation.Authorization.Services;
using VyazmaTech.Prachka.Presentation.Authorization.Tools;

namespace VyazmaTech.Prachka.Presentation.Authorization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVyazmaTechAuthorization(this IServiceCollection services)
    {
        services.AddOptions<AuthorizationConfiguration>().BindConfiguration(AuthorizationConfiguration.SectionKey);

        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizeFeaturePolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, FeatureAuthorizationHandler>();
        services.AddSingleton<IFeatureService, FeatureService>();

        return services;
    }
}