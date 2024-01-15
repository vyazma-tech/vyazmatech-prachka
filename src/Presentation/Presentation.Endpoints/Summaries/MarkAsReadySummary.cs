using FastEndpoints;
using Presentation.Endpoints.Order.MarkAsReady;
using static Application.Handlers.Order.Commands.MarkOrderAsReady.MarkOrderAsReady;

namespace Presentation.Endpoints.Summaries;

internal class MarkAsReadySummary : Summary<MarkAsReadyEndpoint, Command>
{
    public MarkAsReadySummary()
    {
        Summary = "mark as ready";
        Description = "mark specified order as ready";
        RequestParam(
            r => r.Id,
            "the ID must belong to an existing order");
        Response(200, "order successfully marked as ready");
        Response(404, "Error: Order not found");
        Response(400, "Error: Order was ready");
    }
}