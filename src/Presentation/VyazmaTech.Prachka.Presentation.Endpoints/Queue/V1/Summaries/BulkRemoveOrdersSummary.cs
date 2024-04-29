using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Summaries;

internal sealed class BulkRemoveOrdersSummary : Summary<BulkRemoveOrdersEndpoint>
{
    public BulkRemoveOrdersSummary()
    {
        Summary = "Exit queue with several orders";
        Description = "The endpoint removes several orders for current user in specified queue";
        Response();
        Response<ValidationProblemDetails>(StatusCodes.Status401Unauthorized, "You are not authorized");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest);
    }
}