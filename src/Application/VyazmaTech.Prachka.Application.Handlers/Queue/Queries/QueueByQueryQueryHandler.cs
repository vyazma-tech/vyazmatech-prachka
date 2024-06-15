using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Queries;

internal sealed class QueueByQueryQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;
    private readonly IDateTimeProvider _timerProvider;

    public QueueByQueryQueryHandler(
        IPersistenceContext persistenceContext,
        IOptions<PaginationConfiguration> paginationConfiguration,
        IDateTimeProvider timerProvider)
    {
        _persistenceContext = persistenceContext;
        _timerProvider = timerProvider;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        QueueQuery query = QueueQuery.Builder()
            .WithSearchFromDate(request.AssignmentDate ?? _timerProvider.DateNow)
            .WithLimit(_recordsPerPage)
            .WithPage(request.Page)
            .Build();

        long totalCount = await _persistenceContext.Queues.CountAsync(query, cancellationToken);

        List<QueueDto> queues = await _persistenceContext.Queues
            .QueryFromAsync(query)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var page = queues.ToPagedResponse(
            currentPage: request.Page,
            recordsPerPage: _recordsPerPage,
            totalPages: (totalCount / _recordsPerPage) + 1);

        return new Response(page);
    }
}