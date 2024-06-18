using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Core.Order;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Summaries;

internal sealed class ProlongOrderSummary : Summary<ProlongOrderEndpoint>
{
    public ProlongOrderSummary()
    {
        Summary = "Prolong order";
        Description = "This endpoint prolong order with specified id of order and id of target queue";
        Response<OrderDto>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}