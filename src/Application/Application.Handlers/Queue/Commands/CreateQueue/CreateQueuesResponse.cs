namespace Application.Handlers.Queue.Commands.CreateQueue;

public record struct QueueResponseModel(
    Guid Id,
    int Capacity,
    DateTime AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record CreateQueuesResponse(IReadOnlyCollection<QueueResponseModel> Queues);