using Application.Handlers.Order.Commands.MarkOrderAsReady;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.MarkAsReady;

public class MarkAsReadyEndpoint : Endpoint<MarkOrderAsReadyCommand, Result<OrderResponse>>
{
    private readonly IMediator _mediator;

    public MarkAsReadyEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/order/mark-as-ready");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MarkOrderAsReadyCommand req, CancellationToken ct)
    {
        Result<OrderResponse> response = await _mediator.Send(req, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response, ct);
        }
        else
        {
            AddError(response.Error.Message);
            await SendNotFoundAsync(ct);
        }
    }
}