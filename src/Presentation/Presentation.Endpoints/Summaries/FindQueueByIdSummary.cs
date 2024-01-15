using FastEndpoints;
using Presentation.Endpoints.Queue.FindQueue;
using static Application.Handlers.Queue.Queries.QueueById.QueueByIdQuery;

namespace Presentation.Endpoints.Summaries;

internal class FindQueueByIdSummary : Summary<FindQueueByIdEndpoint, Query>
{
    public FindQueueByIdSummary()
    {
        Summary = "Provide searching queue by id";
        RequestParam(
            r => r.Id,
            "the ID must belong to an existing queue");
        Response(404, "Error: Queue not found");
    }
}