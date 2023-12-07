namespace Application.Handlers.Queue.Commands.CreateQueue;

public record struct QueueResponseModel(
    Guid Id,
    int Capacity,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record CreateQueuesResponse(IReadOnlyCollection<QueueResponseModel> Queues);