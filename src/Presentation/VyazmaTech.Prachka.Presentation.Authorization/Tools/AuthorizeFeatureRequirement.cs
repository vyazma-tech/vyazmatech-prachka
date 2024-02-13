using Microsoft.AspNetCore.Authorization;

namespace VyazmaTech.Prachka.Presentation.Authorization.Tools;

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