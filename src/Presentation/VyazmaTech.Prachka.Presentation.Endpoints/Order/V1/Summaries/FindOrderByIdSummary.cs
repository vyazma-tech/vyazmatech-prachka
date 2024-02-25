using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Order;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Summaries;

internal sealed class FindOrderByIdSummary : Summary<FindOrderByIdEndpoint>
{
    public FindOrderByIdSummary()
    {
        Summary = "Find order by id";
        Response<OrderDto>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "Order with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}