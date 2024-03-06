using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.ProlongOrder;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1;

internal class ProlongOrderEndpoint : Endpoint<ProlongOrderRequest, OrderDto>
{
    private const string FeatureName = "Prolong";
    private readonly ISender _sender;

    public ProlongOrderEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("prolong");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<OrderEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(ProlongOrderRequest req, CancellationToken ct)
    {
        var command = new Command(req.OrderId, req.QueueId);
        Result<Response> response = await _sender.Send(command, ct);

        await response.Match(
            success => SendOkAsync(success.Order, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}