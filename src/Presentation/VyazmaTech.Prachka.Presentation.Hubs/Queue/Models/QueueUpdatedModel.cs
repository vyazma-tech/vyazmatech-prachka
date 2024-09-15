namespace VyazmaTech.Prachka.Presentation.Hubs.Queue.Models;

public sealed record QueueUpdatedModel(
    Guid Id,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil,
    string State,
    int CurrentCapacity,
    int MaxCapacity);