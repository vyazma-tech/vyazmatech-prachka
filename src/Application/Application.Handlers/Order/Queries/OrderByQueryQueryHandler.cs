using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts.Common;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts;
using Application.DataAccess.Contracts.Querying.Order;
using Domain.Core.Order;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Orders.Queries.OrderByQuery;

namespace Application.Handlers.Order.Queries;

internal sealed class OrderByQueryQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
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

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, OrderQuery.Builder());
        OrderQuery query = builder.Build();

        long totalCount = await _persistenceContext.Orders.CountAsync(query, cancellationToken);

        List<OrderEntity> orders = await _persistenceContext.Orders
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        Response[] result = orders.Select(x => x.ToDto()).ToArray();

        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = query.Page + 1 ?? 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalCount / _recordsPerPage) + 1,
        };
    }
}