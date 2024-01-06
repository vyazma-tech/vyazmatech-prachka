using FastEndpoints;
using Mediator;
using static Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries.ChangeQueueActivityBoundaries;

namespace Presentation.Endpoints.Queue.ChangeQueueActivityBoundaries;

public class ChangeQueueActivityBoundariesEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public ChangeQueueActivityBoundariesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/api/queues");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        Response response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}