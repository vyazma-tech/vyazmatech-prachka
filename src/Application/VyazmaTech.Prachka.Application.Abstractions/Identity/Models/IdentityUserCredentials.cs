namespace VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

public record IdentityUserCredentials(
    string TelegramUsername,
    string Fullname,
    string TelegramId,
    string TelegramImageUrl);