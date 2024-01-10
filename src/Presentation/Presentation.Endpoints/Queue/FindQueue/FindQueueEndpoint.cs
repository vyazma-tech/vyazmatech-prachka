using Application.Core.Common;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Queue.Queries.QueueByQuery.QueueByQueryQuery;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueEndpoint : Endpoint<Query, PagedResponse<Response>>
{
    private readonly IMediator _mediator;

    public FindQueueEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/queues");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        PagedResponse<Response> response = await _mediator.Send(req, ct);

        if (response.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}