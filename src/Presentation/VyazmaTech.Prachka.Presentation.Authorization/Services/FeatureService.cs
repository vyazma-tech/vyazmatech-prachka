using System.Security.Claims;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Presentation.Authorization.Contracts;
using VyazmaTech.Prachka.Presentation.Authorization.Models;

namespace VyazmaTech.Prachka.Presentation.Authorization.Services;

internal sealed class FeatureService : IFeatureService
{
    private readonly IOptionsMonitor<AuthorizationConfiguration> _configuration;
    private readonly ILogger<FeatureService> _logger;

    public FeatureService(IOptionsMonitor<AuthorizationConfiguration> configuration, ILogger<FeatureService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public bool HasFeature(ClaimsPrincipal principal, string scope, string feature)
    {
        if (principal.Identity?.IsAuthenticated is false)
        {
            return false;
        }

        FeatureScopes featureScopes = _configuration.CurrentValue.FeatureScopes;

        if (featureScopes.TryGetValue(scope, out FeatureScope? featureScope) is false)
        {
            _logger.LogWarning("Feature scope = {Scope} not found", scope);
            return false;
        }

        if (featureScope.TryGetValue(feature, out FeatureRoles? featureRoles) is false)
        {
            _logger.LogWarning("Feature = {Feature} for scope = {Scope}", feature, scope);
            return false;
        }

        return principal.Claims
            .Where(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Value)
            .Intersect(featureRoles, StringComparer.OrdinalIgnoreCase)
            .Any();
    }
}