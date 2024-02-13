using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order;

internal class MarkAsPaidEndpoint : Endpoint<OrderWithIdRequest, OrderDto>
{
    private readonly ISender _sender;

    public MarkAsPaidEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/api/orders/paid");
        AllowAnonymous();
    }

    public override async Task HandleAsync(OrderWithIdRequest req, CancellationToken ct)
    {
        var command = new MarkOrderAsPaid.Command(req.OrderId);

        Result<MarkOrderAsPaid.Response> response = await _sender.Send(command, ct);

        await response.Match(
            success => SendOkAsync(success.Order, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}