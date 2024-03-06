namespace VyazmaTech.Prachka.Presentation.WebAPI.Models;

internal sealed class AdminModel
{
    public const string SectionKey = "DefaultAdmins";

    public string Username { get; init; } = string.Empty;

    public string Fullname { get; init; } = string.Empty;

    public string TelegramId { get; init; } = string.Empty;

    public string TelegramImageUrl { get; init; } = string.Empty;
}