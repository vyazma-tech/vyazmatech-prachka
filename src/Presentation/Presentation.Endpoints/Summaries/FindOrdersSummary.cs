using FastEndpoints;
using Presentation.Endpoints.Order.FindOrders;

namespace Presentation.Endpoints.Summaries;

internal class FindOrdersSummary : Summary<FindOrdersEndpoint>
{
    public FindOrdersSummary()
    {
        Summary = "list all orders endpoint";
        Description = "lists all orders from desired page with a bunch of 10 by application defaults";
        Response(206, "current page has no content");
    }
}