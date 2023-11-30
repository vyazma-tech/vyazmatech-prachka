namespace Application.Handlers.Queue.Queries;

public record struct QueueResponseModel(
    Guid Id,
    int Capacity,
    DateTime AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record QueueResponse(QueueResponseModel Queue);