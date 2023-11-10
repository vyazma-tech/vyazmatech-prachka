using Domain.Common.Result;
using Domain.Core.Order;

namespace Domain.Core.Queue;

public interface IQueueRepository
{
    Task<Result<QueueEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<QueueEntity>> FindByOrderAsync(OrderEntity order, CancellationToken cancellationToken);

    Task<Result<QueueEntity>> FindByCreationDate(DateTime creationDateUtc, CancellationToken cancellationToken);

    Task InsertRangeAsync(IReadOnlyCollection<QueueEntity> queues, CancellationToken cancellationToken);

    void Update(QueueEntity queue);

    Task<long> CountAsync(CancellationToken cancellationToken);
}