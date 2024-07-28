namespace VyazmaTech.Prachka.Application.Dto.Core.Order;

public record OrderDto(
    Guid Id,
    Guid UserId,
    string TelegramUsername,
    string Fullname,
    Guid QueueId,
    string Status,
    DateOnly CreationDate,
    DateTime? ModifiedOn,
    double? Price);