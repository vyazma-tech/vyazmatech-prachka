using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Order.Commands.MarkOrderAsPaid.MarkOrderAsPaid;

namespace Presentation.Endpoints.Order.MarkAsPaid;

internal class MarkAsPaidEndpoint : Endpoint<Command, Response>
{
    private readonly ISender _sender;

    public MarkAsPaidEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("api/orders/{id}/paid");
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