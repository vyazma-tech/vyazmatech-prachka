using Application.Core.Contracts;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

public sealed class IncreaseQueueCapacityCommand : ICommand<Result<QueueResponse>>
{
    public IncreaseQueueCapacityCommand(Guid queueId, int capacity)
    {
        QueueId = queueId;
        Capacity = capacity;
    }

    public Guid QueueId { get; set; }
    public int Capacity { get; set; }
}