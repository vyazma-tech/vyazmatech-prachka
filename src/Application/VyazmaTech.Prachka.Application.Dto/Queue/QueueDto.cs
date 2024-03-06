namespace VyazmaTech.Prachka.Application.Dto.Queue;

public record QueueDto(
    Guid Id,
    int MaxCapacity,
    int CurrentCapacity,
    string State,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil,
    DateTime? ModifiedOn);