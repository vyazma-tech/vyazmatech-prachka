using Application.Core.Common;
using Application.Core.Querying.Abstractions;
using Application.Handlers.Order.Queries;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.FindOrders;

internal class FindOrdersEndpoint : Endpoint<QueryConfiguration<OrderQueryParameter>, PagedResponse<OrderResponse>>
{
    private readonly IMediator _mediator;

    // TODO: FIX IT
    // private readonly IEntityQuery<OrderQuery.QueryBuilder, OrderQueryParameter> _query;
    public FindOrdersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/orders/query");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryConfiguration<OrderQueryParameter> configuration, CancellationToken ct)
    {
        // TODO: fix it
        // OrderQuery.QueryBuilder queryBuilder = OrderQuery.Builder;
        // queryBuilder = _query.Apply(queryBuilder, configuration);
        //
        // OrderQuery orderQuery = queryBuilder.Build();
        //
        // PagedResponse<OrderResponse> response = await _mediator.Send(orderQuery, ct);
        await SendOkAsync();
    }
}