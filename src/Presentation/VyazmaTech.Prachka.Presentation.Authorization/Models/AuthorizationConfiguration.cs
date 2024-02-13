namespace VyazmaTech.Prachka.Presentation.Authorization.Models;

internal sealed class AuthorizationConfiguration
{
    public const string SectionKey = "Authorization";

    public FeatureScopes FeatureScopes { get; set; } = new FeatureScopes();
}