using FastEndpoints;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue;

internal sealed class QueueEndpointGroup : Group
{
    public QueueEndpointGroup()
    {
        Configure("queues", _ => { });
    }
}