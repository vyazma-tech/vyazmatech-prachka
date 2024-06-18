using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IQueueRepository
{
    Task<Queue> GetByIdAsync(Guid id, CancellationToken token);

    Task<Queue?> FindByAssignmentDate(AssignmentDate assignmentDate, CancellationToken token);

    IAsyncEnumerable<Queue> QueryByTelegramUsername(TelegramUsername username, DateOnly searchFrom);

    IAsyncEnumerable<Queue> QueryFromAsync(QueueByQuery.Query specification);

    void InsertRange(IReadOnlyCollection<Queue> queues);

    Task<long> CountAsync(QueueByQuery.Query specification, CancellationToken cancellationToken);
}