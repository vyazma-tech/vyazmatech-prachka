using Application.Core.Common;
using Application.Handlers.Order.Queries;
using Application.Handlers.Order.Queries.FindOrder;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.FindOrders;

internal class FindOrdersEndpoint : Endpoint<FindOrderQuery, PagedResponse<OrderResponse>>
{
    private readonly IMediator _mediator;

    public FindOrdersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FindOrderQuery orderQuery, CancellationToken ct)
    {
        PagedResponse<OrderResponse> response = await _mediator.Send(orderQuery, ct);
        await SendOkAsync(response);
    }
}