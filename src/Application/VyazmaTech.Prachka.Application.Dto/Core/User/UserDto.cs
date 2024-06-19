namespace VyazmaTech.Prachka.Application.Dto.Core.User;

public record UserDto(
    Guid Id,
    string TelegramUsername,
    string Fullname,
    DateTime? ModifiedOn,
    DateOnly RegistrationDate);