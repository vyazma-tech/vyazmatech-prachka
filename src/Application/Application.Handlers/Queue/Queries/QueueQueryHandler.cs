using Application.Core.Contracts;
using Application.Core.Querying.Abstractions;
using Application.DataAccess.Contracts;
using Application.Handlers.Mapping.QueueMapping;
using Domain.Core.Queue;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Queue.Queries;

internal sealed class QueueQueryHandler : IQueryHandler<QueueQuery, QueueResponse>
{
    private readonly IQueueRepository _queueRepository;
    private readonly DbSet<QueueEntity> _queues;
    private readonly IEntityQuery<IQueryable<QueueEntity>, QueueQuery> _query;

    public QueueQueryHandler(
        IPersistenceContext persistenceContext,
        IEntityQuery<IQueryable<QueueEntity>, QueueQuery> query)
    {
        _queueRepository = persistenceContext.Queues;
        _queues = persistenceContext.Entities<QueueEntity>();
        _query = query;
    }

    public async ValueTask<QueueResponse> Handle(QueueQuery request, CancellationToken cancellationToken)
    {
        IQueryable<QueueEntity> inputQueryable = _query
            .Apply(_queues, new QueryConfiguration<QueueQuery>(request));

        List<QueueEntity> result = await inputQueryable.ToListAsync(cancellationToken);

        return new QueueResponse(result.Select(x => x.ToDto()).ToArray());
    }
}