using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IQueueRepository
{
    IAsyncEnumerable<Queue> QueryAsync(
        QueueQuery specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<Queue> queues);

    void Update(Queue queue);

    Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken);
}