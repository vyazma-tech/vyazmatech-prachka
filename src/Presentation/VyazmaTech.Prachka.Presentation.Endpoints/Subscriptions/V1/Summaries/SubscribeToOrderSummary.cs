using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Subscriptions.V1.Summaries;

internal sealed class SubscribeToOrderSummary : Summary<SubscribeToOrderEndpoint>
{
    public SubscribeToOrderSummary()
    {
        Summary = "Subscribes current user on order news";
        Response();
        Response(StatusCodes.Status401Unauthorized, "You are not authorized");
    }
}