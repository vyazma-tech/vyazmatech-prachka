namespace VyazmaTech.Prachka.Presentation.Endpoints.User.Models;

internal sealed record FindUsersRequest(
    string? TelegramId,
    string? Fullname,
    DateOnly? RegistrationDate,
    int? Page);