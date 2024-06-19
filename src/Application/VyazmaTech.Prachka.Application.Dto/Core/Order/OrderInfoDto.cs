namespace VyazmaTech.Prachka.Application.Dto.Core.Order;

public record OrderInfoDto(Guid OrderId, Guid UserId, string TelegramUsername, string Fullname, string Status);