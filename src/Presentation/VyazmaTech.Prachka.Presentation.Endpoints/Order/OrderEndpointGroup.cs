using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order;

internal sealed class OrderEndpointGroup : Group
{
    public OrderEndpointGroup()
    {
        Configure("orders", _ => { });
    }
}