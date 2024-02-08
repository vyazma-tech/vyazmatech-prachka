using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Presentation.Authorization.Tools;

internal sealed class AuthorizeFeaturePolicyProvider : IAuthorizationPolicyProvider
{
    private readonly IAuthorizationPolicyProvider _defaultProvider;

    public AuthorizeFeaturePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _defaultProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(AuthorizeFeatureAttribute.Prefix, StringComparison.OrdinalIgnoreCase) is false)
            return _defaultProvider.GetPolicyAsync(policyName);

        var builder = new AuthorizationPolicyBuilder();

        int start = AuthorizeFeatureAttribute.Prefix.Length;
        int separator = policyName.IndexOf(":", start, StringComparison.OrdinalIgnoreCase);

        if (separator < 0)
            return _defaultProvider.GetPolicyAsync(policyName);

        string scope = policyName[start..separator];
        string feature = policyName[(separator + 1)..];

        builder.AddRequirements(new AuthorizeFeatureRequirement(scope, feature));

        AuthorizationPolicy policy = builder.Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _defaultProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _defaultProvider.GetFallbackPolicyAsync();
}