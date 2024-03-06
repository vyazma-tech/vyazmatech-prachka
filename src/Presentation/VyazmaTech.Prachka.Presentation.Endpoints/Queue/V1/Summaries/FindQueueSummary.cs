using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Summaries;

internal sealed class FindQueueSummary : Summary<FindQueueEndpoint>
{
    public FindQueueSummary()
    {
        Summary = "Find queues by query";
        Description = @"Returns a bunch of 10 queues by the application defaults from the first page.
                              Default page is 0.";
        Response<PagedResponse<QueueDto>>(StatusCodes.Status206PartialContent);
        Response(StatusCodes.Status204NoContent, "Nothing found on specified page.");
        Response<ProblemDetails>(StatusCodes.Status400BadRequest, "Input was not in correct format.");
    }
}