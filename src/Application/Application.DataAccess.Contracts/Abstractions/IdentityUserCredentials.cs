namespace Application.DataAccess.Contracts.Abstractions;

public record IdentityUserCredentials(
    string TelegramUsername,
    string Fullname,
    string TelegramId,
    string TelegramImageUrl);