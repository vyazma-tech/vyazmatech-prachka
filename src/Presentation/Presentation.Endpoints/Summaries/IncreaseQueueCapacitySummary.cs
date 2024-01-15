using FastEndpoints;
using Presentation.Endpoints.Queue.IncreaseQueueCapacity;
using static Application.Handlers.Queue.Commands.IncreaseQueueCapacity.IncreaseQueueCapacity;

namespace Presentation.Endpoints.Summaries;

internal class IncreaseQueueCapacitySummary : Summary<IncreaseQueueCapacityEndpoint, Command>
{
    public IncreaseQueueCapacitySummary()
    {
        Summary = "increase capacity";
        Description = "increase capacity of specified queue";
        RequestParam(
            r => r.Capacity,
            "capacity should be at least zero and should not be less then current capacity");
        RequestParam(
            r => r.QueueId,
            "the ID must belong to an existing queue");
        Response(200, "queue capacity successfully increased");
        Response(404, "Error: Queue not found");
        Response(400, "Error: validation failure");
    }
}