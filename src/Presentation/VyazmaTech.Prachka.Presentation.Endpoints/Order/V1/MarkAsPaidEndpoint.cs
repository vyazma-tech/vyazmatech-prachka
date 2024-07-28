using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands;
using VyazmaTech.Prachka.Application.Dto.Core.Order;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1;

internal class MarkAsPaidEndpoint : Endpoint<MarkAsPaidRequest, OrderDto>
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

    public override async Task HandleAsync(MarkAsPaidRequest req, CancellationToken ct)
    {
        var command = new MarkOrderAsPaid.Command(req.OrderId, req.Price);

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