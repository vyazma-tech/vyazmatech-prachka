namespace VyazmaTech.Prachka.Presentation.Authorization.Models;

internal sealed class FeatureScope : Dictionary<string, FeatureRoles>
{
    public FeatureScope()
        : base(StringComparer.OrdinalIgnoreCase) { }
}