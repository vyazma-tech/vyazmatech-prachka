using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Subscriptions;

internal sealed class SubscriptionEndpointGroup : Group
{
    public SubscriptionEndpointGroup()
    {
        Configure("subscriptions", _ => { });
    }
}