using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Order.MarkAsReady;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsReady;

namespace Presentation.Endpoints.Order.Summaries;

internal sealed class MarkAsReadySummary : Summary<MarkAsReadyEndpoint>
{
    public MarkAsReadySummary()
    {
        Summary = "Mark order as ready.";
        Description = "This endpoint marks order with specified id as ready.";
        Response<Response>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}