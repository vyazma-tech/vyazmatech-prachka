using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.Queue;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Queries;

internal sealed class QueueByQueryQueryHandler : IQueryHandler<Query, Response>
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

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, QueueQuery.Builder());
        QueueQuery query = builder.Build();

        long totalCount = await _persistenceContext.Queues.CountAsync(query, cancellationToken);

        List<QueueEntity> queues = await _persistenceContext.Queues
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        IEnumerable<QueueDto> result = queues.Select(x => x.ToDto());

        var page = result.ToPagedResponse(
            currentPage: query.Page + 1 ?? 1,
            recordsPerPage: _recordsPerPage,
            totalPages: (totalCount / _recordsPerPage) + 1);

        return new Response(page);
    }
}