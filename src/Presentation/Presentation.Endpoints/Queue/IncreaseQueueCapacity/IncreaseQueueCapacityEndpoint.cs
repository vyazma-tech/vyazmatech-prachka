using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Queue.Commands.IncreaseQueueCapacity.IncreaseQueueCapacity;

namespace Presentation.Endpoints.Queue.IncreaseQueueCapacity;

internal class IncreaseQueueCapacityEndpoint : Endpoint<Command, Response>
{
    private readonly ISender _sender;

    public IncreaseQueueCapacityEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("api/queues/{queueId}/capacity/{capacity}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        Result<Response> response = await _sender.Send(req, ct);

        await response.Match(
            success => SendOkAsync(success, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}