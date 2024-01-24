using Application.DataAccess.Contracts.Querying.Queue;
using Domain.Core.Queue;

namespace Application.DataAccess.Contracts.Repositories;

public interface IQueueRepository
{
    IAsyncEnumerable<QueueEntity> QueryAsync(
        QueueQuery specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<QueueEntity> queues);

    void Update(QueueEntity queue);

    Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken);
}