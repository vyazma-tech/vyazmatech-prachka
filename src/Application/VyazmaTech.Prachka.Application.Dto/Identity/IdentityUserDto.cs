namespace VyazmaTech.Prachka.Application.Dto.Identity;

public record IdentityUserDto(
    Guid Id,
    string Fullname,
    string TelegramUsername,
    string TelegramImageUrl);