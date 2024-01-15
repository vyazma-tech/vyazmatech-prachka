using FastEndpoints;
using Presentation.Endpoints.Queue.FindQueue;

namespace Presentation.Endpoints.Summaries;

internal class FindQueueSummary : Summary<FindQueueEndpoint>
{
    public FindQueueSummary()
    {
        Summary = "list all queues endpoint";
        Description = "lists all queues from desired page with a bunch of 10 by application defaults";
        Response(206, "current page has no content");
    }
}