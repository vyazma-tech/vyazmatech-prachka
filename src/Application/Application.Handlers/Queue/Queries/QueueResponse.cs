namespace Application.Handlers.Queue.Queries;

public record struct QueueResponseModel(
    Guid Id,
    int Capacity,
    bool Expired,
    DateOnly AssignmentDate,
    DateTime? ModifiedOn,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil);

public sealed record QueueResponse(QueueResponseModel Queue);