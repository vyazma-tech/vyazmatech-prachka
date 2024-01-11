using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Core.Extensions;
using Domain.Core.Order;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.Extensions.Options;
using static Application.Handlers.Order.Queries.OrderByQuery.OrderByQuery;

namespace Application.Handlers.Order.Queries.OrderByQuery;

internal sealed class OrderByQueryQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;

    public OrderByQueryQueryHandler(
        IOptions<PaginationConfiguration> paginationConfiguration,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        var spec = new OrderByPageSpecification(request.Page, _recordsPerPage);
        long totalCount = await _persistenceContext.Orders.CountAsync(spec, cancellationToken);

        List<OrderEntity> queues = await _persistenceContext.Orders
            .QueryAsync(spec, cancellationToken)
            .ToListAsync(cancellationToken);

        Response[] result = queues
            .FilterBy(
                request.UserId,
                request.QueueId,
                request.CreationDate)
            .Select(x => x.ToDto())
            .ToArray();

        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = request.Page + 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalCount / _recordsPerPage) + 1
        };
    }
}