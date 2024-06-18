namespace VyazmaTech.Prachka.Presentation.Endpoints.User.V1.Models;

internal sealed record FindUsersRequest(
    string? TelegramId,
    string? Fullname,
    DateOnly? RegistrationDate,
    int Page,
    int? Limit);