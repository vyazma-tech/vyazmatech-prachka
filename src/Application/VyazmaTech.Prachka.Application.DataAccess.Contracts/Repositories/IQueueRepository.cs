using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Domain.Core.Queue;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IQueueRepository
{
    IAsyncEnumerable<QueueEntity> QueryAsync(
        QueueQuery specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<QueueEntity> queues);

    void Update(QueueEntity queue);

    Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken);
}