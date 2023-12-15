using Application.Core.Common;
using Application.Handlers.Order.Queries;
using FastEndpoints;
using Infrastructure.DataAccess.Quering.Abstractions;
using Mediator;

namespace Presentation.Endpoints.Order.FindOrders;

internal class FindOrdersEndpoint : Endpoint<QueryConfiguration<OrderQueryParameter>, PagedResponse<OrderResponse>>
{
    private readonly IMediator _mediator;
    private readonly IModelQuery<OrderQuery.QueryBuilder, OrderQueryParameter> _query;

    public FindOrdersEndpoint(IMediator mediator, IModelQuery<OrderQuery.QueryBuilder, OrderQueryParameter> query)
    {
        _mediator = mediator;
        _query = query;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/orders/query");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryConfiguration<OrderQueryParameter> configuration, CancellationToken ct)
    {
        OrderQuery.QueryBuilder queryBuilder = OrderQuery.Builder;
        queryBuilder = _query.Apply(queryBuilder, configuration);

        OrderQuery orderQuery = queryBuilder.Build();

        PagedResponse<OrderResponse> response = await _mediator.Send(orderQuery, ct);
        await SendOkAsync(response);
    }
}