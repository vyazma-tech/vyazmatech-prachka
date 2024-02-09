using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.Order.ProlongOrder;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsPaid;
namespace Presentation.Endpoints.Order.Summaries;

internal sealed class ProlongOrderSummary : Summary<ProlongOrderEndpoint>
{
    public ProlongOrderSummary()
    {
        Summary = "Prolong order";
        Description = "This endpoint prolong order with specified id of order and id of target queue";
        Response<Response>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}