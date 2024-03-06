using Microsoft.AspNetCore.Authorization;

namespace VyazmaTech.Prachka.Presentation.Authorization;

public sealed class AuthorizeFeatureAttribute : AuthorizeAttribute
{
    public const string Prefix = "Feature_";

    public AuthorizeFeatureAttribute(string scope, string feature)
        : base($"{Prefix}{scope}:{feature}")
    {
        Scope = scope;
        Feature = feature;
    }

    public string Scope { get; }

    public string Feature { get; }
}