using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries;
using VyazmaTech.Prachka.Application.Dto.Core.Order;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1;

internal sealed class MyOrdersEndpoint : EndpointWithoutRequest<MyOrdersDto>
{
    private readonly ISender _sender;

    public MyOrdersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/my-orders");
        Group<OrderEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new MyOrders.Query();
        MyOrders.Response response = await _sender.Send(query, ct);

        if (response.History.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}