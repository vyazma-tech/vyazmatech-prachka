using Microsoft.AspNetCore.Authorization;
using Presentation.Authorization.Contracts;
using Presentation.Authorization.Models;
using Presentation.Authorization.Services;
using Presentation.Authorization.Tools;

namespace Presentation.Authorization;

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