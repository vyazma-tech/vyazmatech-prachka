using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Core.Contracts.Orders.Commands.ProlongOrder;

namespace Presentation.Endpoints.Order.ProlongOrder;

internal class ProlongOrderEndpoint : Endpoint<Command, Response>
{
    private readonly ISender _sender;

    public ProlongOrderEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/api/orders/prolong");
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