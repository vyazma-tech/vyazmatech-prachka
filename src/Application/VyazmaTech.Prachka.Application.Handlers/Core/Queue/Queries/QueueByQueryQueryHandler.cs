using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Queries;

internal sealed class QueueByQueryQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public QueueByQueryQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        long totalCount = await _persistenceContext.Queues.CountAsync(request, cancellationToken);

        List<QueueDto> queues = await _persistenceContext.Queues
            .QueryFromAsync(request)
            .OrderBy(x => x.AssignmentDate.Value)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var page = queues.ToPagedResponse(
            currentPage: request.Page,
            recordsPerPage: request.Limit,
            totalPages: (totalCount / request.Limit) + 1);

        return new Response(page);
    }
}