namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

internal sealed record RegisterResponse(
    Guid Id,
    string Fullname,
    string Role,
    string TelegramUsername,
    string TelegramImageUrl,
    string AccessToken,
    string RefreshToken);