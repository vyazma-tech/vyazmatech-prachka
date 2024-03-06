namespace VyazmaTech.Prachka.Application.Dto.Order;

public record OrderInfoDto(Guid OrderId, Guid UserId, string TelegramUsername, string Fullname, string Status);