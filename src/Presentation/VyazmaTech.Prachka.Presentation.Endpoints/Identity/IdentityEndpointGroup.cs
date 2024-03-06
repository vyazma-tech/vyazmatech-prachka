using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal sealed class IdentityEndpointGroup : Group
{
    public IdentityEndpointGroup()
    {
        Configure("identity", _ => { });
    }
}