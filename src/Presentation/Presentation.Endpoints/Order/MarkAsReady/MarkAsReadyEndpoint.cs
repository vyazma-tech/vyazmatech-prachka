using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Order.Commands.MarkOrderAsReady.MarkOrderAsReady;

namespace Presentation.Endpoints.Order.MarkAsReady;

internal class MarkAsReadyEndpoint : Endpoint<Command, Response>
{
    private readonly ISender _sender;

    public MarkAsReadyEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/api/orders/{id}/ready");
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