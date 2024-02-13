namespace VyazmaTech.Prachka.Presentation.Authorization.Models;

internal sealed class FeatureScopes : Dictionary<string, FeatureScope>
{
    public FeatureScopes()
        : base(StringComparer.OrdinalIgnoreCase)
    {
    }
}