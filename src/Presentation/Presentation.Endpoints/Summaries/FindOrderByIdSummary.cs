using Application.Handlers.Order.Queries.OrderById;
using FastEndpoints;
using Presentation.Endpoints.Order.FindOrders;

namespace Presentation.Endpoints.Summaries;

internal class FindOrderByIdSummary : Summary<FindOrderByIdEndpoint, OrderByIdQuery.Query>
{
    public FindOrderByIdSummary()
    {
        Summary = "Provide searching order by id";
        RequestParam(
            r => r.Id,
            "the ID must belong to an existing order");
        Response(404, "Error: Order not found");
    }
}