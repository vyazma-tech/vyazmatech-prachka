namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

internal sealed record RegisterRequest(
    string TelegramUsername,
    string Fullname,
    string TelegramId,
    string TelegramImageUrl,
    string Role);