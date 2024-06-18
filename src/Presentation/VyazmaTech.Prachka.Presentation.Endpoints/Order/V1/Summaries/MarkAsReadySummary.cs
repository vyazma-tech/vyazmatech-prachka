using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Core.Order;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Summaries;

internal sealed class MarkAsReadySummary : Summary<MarkAsReadyEndpoint>
{
    public MarkAsReadySummary()
    {
        Summary = "Mark order as ready.";
        Description = "This endpoint marks order with specified id as ready.";
        Response<OrderDto>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Parameters were not in correct format");
    }
}