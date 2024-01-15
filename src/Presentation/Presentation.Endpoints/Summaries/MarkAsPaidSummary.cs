using FastEndpoints;
using Presentation.Endpoints.Order.MarkAsPaid;
using static Application.Handlers.Order.Commands.MarkOrderAsPaid.MarkOrderAsPaid;

namespace Presentation.Endpoints.Summaries;

internal class MarkAsPaidSummary : Summary<MarkAsPaidEndpoint, Command>
{
    public MarkAsPaidSummary()
    {
        Summary = "mark as paid";
        Description = "mark specified order as paid";
        RequestParam(
            r => r.Id,
            "the ID must belong to an existing order");
        Response(200, "order successfully marked as paid");
        Response(404, "Error: Order not found");
        Response(400, "Error: Order was already paid");
    }
}