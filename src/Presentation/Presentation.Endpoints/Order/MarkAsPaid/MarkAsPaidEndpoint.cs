using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Order.Commands.MarkOrderAsPaid.MarkOrderAsPaid;

namespace Presentation.Endpoints.Order.MarkAsPaid;

public class MarkAsPaidEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public MarkAsPaidEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("api/order/mark-as-paid");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        Result<Response> response = await _mediator.Send(req, ct);

        await response.Match(
            success => SendOkAsync(success, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}