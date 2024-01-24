using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts.Common;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts;
using Application.DataAccess.Contracts.Querying.Queue;
using Domain.Core.Queue;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Queues.Queries.QueueByQuery;

namespace Application.Handlers.Queue.Queries;

internal sealed class QueueByQueryQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;
    private readonly IQueryProcessor<Query, IQueryBuilder> _queryProcessor;

    public QueueByQueryQueryHandler(
        IPersistenceContext persistenceContext,
        IOptions<PaginationConfiguration> paginationConfiguration,
        IQueryProcessor<Query, IQueryBuilder> queryProcessor)
    {
        _persistenceContext = persistenceContext;
        _queryProcessor = queryProcessor;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, QueueQuery.Builder());
        QueueQuery query = builder.Build();

        long totalCount = await _persistenceContext.Queues.CountAsync(query, cancellationToken);

        List<QueueEntity> queues = await _persistenceContext.Queues
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        Response[] result = queues.Select(x => x.ToDto()).ToArray();

        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = query.Page + 1 ?? 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalCount / _recordsPerPage) + 1,
        };
    }
}