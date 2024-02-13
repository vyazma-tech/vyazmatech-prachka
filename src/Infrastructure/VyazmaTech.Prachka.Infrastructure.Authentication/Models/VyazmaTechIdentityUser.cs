using Microsoft.AspNetCore.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Models;

internal class VyazmaTechIdentityUser : IdentityUser<Guid>
{
    public VyazmaTechIdentityUser(IdentityUserCredentials credentials)
        : base(credentials.TelegramUsername)
    {
        Fullname = credentials.Fullname;
        TelegramId = credentials.TelegramId;
        TelegramImageUrl = credentials.TelegramImageUrl;
    }

    protected VyazmaTechIdentityUser()
    {
    }

    public string Fullname { get; init; } = string.Empty;

    public string TelegramId { get; init; } = string.Empty;

    public string TelegramImageUrl { get; init; } = string.Empty;

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryUtc { get; init; }
}