using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1;

internal class MarkAsPaidEndpoint : Endpoint<OrderWithIdRequest, OrderDto>
{
    private const string FeatureName = "MarkAsPaid";
    private readonly ISender _sender;

    public MarkAsPaidEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("paid");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<OrderEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(OrderWithIdRequest req, CancellationToken ct)
    {
        var command = new MarkOrderAsPaid.Command(req.OrderId);

        try
        {
            MarkOrderAsPaid.Response response = await _sender.Send(command, ct);
            await SendOkAsync(response.Order, ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}