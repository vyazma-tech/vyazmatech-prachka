namespace VyazmaTech.Prachka.Presentation.WebAPI.Models;

internal sealed class AdminModel
{
    public const string SectionKey = "DefaultAdmins";

    public string Username { get; set; } = string.Empty;

    public string Fullname { get; set; } = string.Empty;

    public string TelegramId { get; set; } = string.Empty;

    public string TelegramImageUrl { get; set; } = string.Empty;
}