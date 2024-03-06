using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.Order;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Queries.OrderByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Queries;

internal sealed class OrderByQueryQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;
    private readonly IQueryProcessor<Query, IQueryBuilder> _queryProcessor;

    public OrderByQueryQueryHandler(
        IOptions<PaginationConfiguration> paginationConfiguration,
        IPersistenceContext persistenceContext,
        IQueryProcessor<Query, IQueryBuilder> queryProcessor)
    {
        _persistenceContext = persistenceContext;
        _queryProcessor = queryProcessor;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, OrderQuery.Builder());
        OrderQuery query = builder.Build();

        long totalCount = await _persistenceContext.Orders.CountAsync(query, cancellationToken);

        List<OrderEntity> orders = await _persistenceContext.Orders
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        IEnumerable<OrderDto> result = orders.Select(x => x.ToDto());

        var page = result.ToPagedResponse(
            currentPage: query.Page + 1 ?? 1,
            recordsPerPage: _recordsPerPage,
            totalPages: (totalCount / _recordsPerPage) + 1);

        return new Response(page);
    }
}