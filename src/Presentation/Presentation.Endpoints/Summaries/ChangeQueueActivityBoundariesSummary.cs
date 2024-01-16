// using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using FastEndpoints;
using Presentation.Endpoints.Queue.ChangeQueueActivityBoundaries;
using static Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries.ChangeQueueActivityBoundaries;

namespace Presentation.Endpoints.Summaries;

internal class ChangeQueueActivityBoundariesSummary : Summary<ChangeQueueActivityBoundariesEndpoint, Command>
{
    public ChangeQueueActivityBoundariesSummary()
    {
        Summary = "change activity boundaries";
        Description = "change activity boundaries of specified queue. " +
                      "The queue activity boundaries should describe time range during the day.";
        RequestParam(
            r => r.QueueId,
            "the ID must belong to an existing queue");
        RequestParam(
            r => r.ActiveFrom,
            "New start time of queue activity. Should not be equal current start time of queue activity");
        RequestParam(
            r => r.ActiveUntil,
            "New end time of queue activity. Should not be equal current end time of queue activity");
        Response(200, "queue activity boundaries successfully changed");
        Response(404, "Error: Queue not found");
        Response(400, "Error: validation failure");
    }
}