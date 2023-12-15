using Application.Core.Contracts;

namespace Application.Handlers.Queue.Commands.CreateQueue;

public readonly record struct QueueModel(
    int Capacity,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed class CreateQueuesCommand : ICommand<CreateQueuesResponse>
{
    public CreateQueuesCommand(IReadOnlyCollection<QueueModel> queues)
    {
        Queues = queues;
    }

    public IReadOnlyCollection<QueueModel> Queues { get; init; }
}