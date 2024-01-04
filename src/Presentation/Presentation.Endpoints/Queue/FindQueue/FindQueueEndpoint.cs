using Application.Core.Querying.Abstractions;
using Application.Handlers.Queue.Queries;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueEndpoint : Endpoint<QueueQuery, QueueResponse>
{
    private readonly IMediator _mediator;

    public FindQueueEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/queue");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueueQuery req, CancellationToken ct)
    {
        QueueResponse response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}