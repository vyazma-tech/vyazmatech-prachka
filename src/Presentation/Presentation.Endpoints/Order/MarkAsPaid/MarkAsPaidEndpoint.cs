using Application.Handlers.Order.Commands.MarkOrderAsPaid;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Order.MarkAsPaid;

public class MarkAsPaidEndpoint : Endpoint<MarkOrderAsPaidCommand, Result<OrderResponse>>
{
    private readonly IMediator _mediator;

    public MarkAsPaidEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/order/mark-as-paid");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MarkOrderAsPaidCommand req, CancellationToken ct)
    {
        Result<OrderResponse> response = await _mediator.Send(req, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response, ct);
        }
        else
        {
            await SendAsync(response, StatusCodes.Status404NotFound, ct);
        }
    }
}