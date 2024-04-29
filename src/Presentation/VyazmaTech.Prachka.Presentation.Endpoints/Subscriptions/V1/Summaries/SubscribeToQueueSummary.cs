using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Subscriptions.V1.Summaries;

internal sealed class SubscribeToQueueSummary : Summary<SubscribeToQueueEndpoint>
{
    public SubscribeToQueueSummary()
    {
        Summary = "Subscribes current user on queues news";
        Response();
        Response(StatusCodes.Status401Unauthorized, "You are not authorized");
    }
}