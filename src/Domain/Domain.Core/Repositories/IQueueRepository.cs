using Domain.Core.Entities;

namespace Domain.Core.Repositories;

public interface IQueueRepository
{
    Task<bool> CheckIfQueuePositionAvailableAsync(Queue queue);

    // Queues later than today or today
    Task<IReadOnlyCollection<Queue>> GetActualQueues();

    void Insert(Queue queue);

    void InsertRange(IReadOnlyCollection<Queue> queues);

    void Remove(Queue queue);
}