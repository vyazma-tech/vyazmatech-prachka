namespace Application.Handlers.Queue.Commands.CreateQueue;

public record struct CreateQueueResponseModel(
    Guid Id,
    int Capacity,
    DateTime AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record CreateQueuesResponse(IReadOnlyCollection<CreateQueueResponseModel> Queues);