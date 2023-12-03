namespace Application.Handlers.Queue.Queries;

public record struct QueueResponseModel(
    Guid Id,
    int Capacity,
    bool Expired,
    DateTime AssignmentDate,
    DateTime? ModifiedOn,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record QueueResponse(QueueResponseModel Queue);