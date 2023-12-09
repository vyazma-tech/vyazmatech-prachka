using Application.Core.Contracts;
using Application.Handlers.Queue.Queries;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

public sealed class IncreaseQueueCapacityCommand : ICommand<Task>
{
    public IncreaseQueueCapacityCommand(Guid queueId, int capacity)
    {
        QueueId = queueId;
        Capacity = capacity;
    }

    public Guid QueueId { get; set; }
    public int Capacity { get; set; }
}