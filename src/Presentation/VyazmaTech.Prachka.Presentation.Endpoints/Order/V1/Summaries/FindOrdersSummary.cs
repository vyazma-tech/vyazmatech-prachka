using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Summaries;

internal sealed class FindOrdersSummary : Summary<FindOrdersEndpoint>
{
    public FindOrdersSummary()
    {
        Summary = "Find orders by query";
        Description = @"Returns a bunch of 10 orders by the application defaults from the first page.
                              Default page is 0.";
        Response<PagedResponse<OrderDto>>(StatusCodes.Status206PartialContent);
        Response(StatusCodes.Status204NoContent, "Nothing found on specified page.");
        Response<ProblemDetails>(StatusCodes.Status400BadRequest, "Input was not in correct format.");
    }
}