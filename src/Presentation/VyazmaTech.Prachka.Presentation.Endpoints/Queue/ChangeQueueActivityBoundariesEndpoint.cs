using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue;

internal class ChangeQueueActivityBoundariesEndpoint : Endpoint<ChangeActivityBoundariesRequest, QueueDto>
{
    private readonly ISender _sender;

    public ChangeQueueActivityBoundariesEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/api/queues/activity");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ChangeActivityBoundariesRequest req, CancellationToken ct)
    {
        var command = new ChangeQueueActivityBoundaries.Command(req.QueueId, req.ActiveFrom, req.ActiveUntil);

        Result<ChangeQueueActivityBoundaries.Response> response = await _sender.Send(command, ct);

        await response.Match(
            success => SendOkAsync(success.Queue, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}