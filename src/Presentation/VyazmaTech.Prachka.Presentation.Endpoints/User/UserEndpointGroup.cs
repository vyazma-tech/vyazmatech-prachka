using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User;

internal sealed class UserEndpointGroup : Group
{
    public UserEndpointGroup()
    {
        Configure("users", _ => { });
    }
}