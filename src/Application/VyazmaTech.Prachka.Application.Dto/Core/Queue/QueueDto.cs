namespace VyazmaTech.Prachka.Application.Dto.Core.Queue;

public record QueueDto(
    Guid Id,
    int MaxCapacity,
    int CurrentCapacity,
    string State,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil,
    DateTime? ModifiedOn,
    bool IsNotifyAvailable);