using Microsoft.AspNetCore.Authorization;

namespace Presentation.Authorization.Tools;

internal sealed class AuthorizeFeatureRequirement : IAuthorizationRequirement
{
    public AuthorizeFeatureRequirement(string scope, string feature)
    {
        Scope = scope;
        Feature = feature;
    }

    public string Scope { get; }

    public string Feature { get; }
}