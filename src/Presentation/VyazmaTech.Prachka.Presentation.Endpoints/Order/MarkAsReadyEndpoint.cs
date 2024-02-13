using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order;

internal class MarkAsReadyEndpoint : Endpoint<OrderWithIdRequest, OrderDto>
{
    private readonly ISender _sender;

    public MarkAsReadyEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/api/orders/ready");
        AllowAnonymous();
    }

    public override async Task HandleAsync(OrderWithIdRequest req, CancellationToken ct)
    {
        var command = new MarkOrderAsReady.Command(req.OrderId);

        Result<MarkOrderAsReady.Response> response = await _sender.Send(command, ct);

        await response.Match(
            success => SendOkAsync(success.Order, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}