using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Summaries;

internal sealed class BulkInsertOrdersSummary : Summary<BulkInsertOrdersEndpoint>
{
    public BulkInsertOrdersSummary()
    {
        Summary = "Enter queue with multiple orders";
        Description = "The endpoint creates and inserts several orders for current user in specified queue";
        Response();
        Response<ValidationProblemDetails>(StatusCodes.Status401Unauthorized, "User not authorized");
        Response<ValidationProblemDetails>(StatusCodes.Status422UnprocessableEntity, "Queue is full");
    }
}