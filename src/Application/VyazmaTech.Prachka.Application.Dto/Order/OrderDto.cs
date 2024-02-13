namespace VyazmaTech.Prachka.Application.Dto.Order;

public record OrderDto(Guid Id, Guid UserId, Guid QueueId, string Status, DateOnly CreationDate, DateTime? ModifiedOn);