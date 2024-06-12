using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Application.Dto.Queue;

public record QueueWithOrdersDto(
    Guid Id,
    int MaxCapacity,
    int CurrentCapacity,
    IReadOnlyCollection<OrderDto> Orders,
    string State,
    DateOnly AssignmentDate,
    TimeOnly ActiveFrom,
    TimeOnly ActiveUntil,
    DateTime? ModifiedOn);