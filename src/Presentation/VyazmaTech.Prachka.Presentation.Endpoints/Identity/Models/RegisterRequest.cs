namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

internal sealed record RegisterRequest(
    string TelegramUsername,
    string Fullname,
    string TelegramId,
    string TelegramImageUrl,
    string Role);