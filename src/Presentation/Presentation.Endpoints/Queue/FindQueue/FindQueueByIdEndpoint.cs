using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using static Application.Handlers.Queue.Queries.QueueById.QueueByIdQuery;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueByIdEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;

    public FindQueueByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/queues/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        Result<Response> response = await _mediator.Send(req, ct);

        await response.Match(
            _ => SendOkAsync(response.Value, ct),
            _ => SendNotFoundAsync(ct));
    }
}