using System.Security.Claims;

namespace VyazmaTech.Prachka.Presentation.Authorization.Contracts;

public interface IFeatureService
{
    bool HasFeature(ClaimsPrincipal principal, string scope, string feature);
}