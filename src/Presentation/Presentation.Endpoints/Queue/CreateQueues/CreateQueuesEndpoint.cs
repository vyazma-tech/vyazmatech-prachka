using Application.Handlers.Queue.Commands.CreateQueue;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Queue.CreateQueues;

internal class CreateQueuesEndpoint : Endpoint<CreateQueuesCommand, CreateQueuesResponse>
{
    private readonly IMediator _mediator;

    public CreateQueuesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/queue");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateQueuesCommand req, CancellationToken ct)
    {
        CreateQueuesResponse response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}