using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IQueueRepository
{
    Task<Queue> GetByIdAsync(Guid id, CancellationToken token);

    Task<Queue?> FindByAssignmentDate(AssignmentDate assignmentDate, CancellationToken token);

    IAsyncEnumerable<Queue> QueryByTelegramUsername(TelegramUsername username, DateOnly searchFrom);

    IAsyncEnumerable<Queue> QueryFromAsync(QueueQuery specification);

    void InsertRange(IReadOnlyCollection<Queue> queues);

    Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken);
}