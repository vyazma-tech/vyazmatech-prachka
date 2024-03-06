using Microsoft.AspNetCore.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models.Events;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Models;

public class VyazmaTechIdentityUser : IdentityUser<Guid>
{
    private readonly List<IIntegrationEvent> _integrationEvents = new ();

    public VyazmaTechIdentityUser(string fullname, string telegramId, string telegramImageUrl, string telegramUsername)
        : base(telegramUsername)
    {
        Fullname = fullname;
        TelegramId = telegramId;
        TelegramImageUrl = telegramImageUrl;
    }

    public static VyazmaTechIdentityUser Create(
        Guid id,
        IdentityUserCredentials credentials,
        string refreshToken,
        DateTime refreshTokenExpiry,
        Guid securityStamp)
    {
        var user = new VyazmaTechIdentityUser(
            credentials.Fullname,
            credentials.TelegramId,
            credentials.TelegramImageUrl,
            credentials.TelegramUsername)
        {
            Id = id,
            RefreshToken = refreshToken,
            RefreshTokenExpiryUtc = refreshTokenExpiry,
            SecurityStamp = securityStamp.ToString()
        };

        user._integrationEvents.Add(new UserRegisteredIntegrationEvent(user));

        return user;
    }

    protected VyazmaTechIdentityUser()
    {
    }

    public string Fullname { get; init; } = string.Empty;

    public string TelegramId { get; init; } = string.Empty;

    public string TelegramImageUrl { get; init; } = string.Empty;

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryUtc { get; init; }

    public IReadOnlyCollection<IIntegrationEvent> IntegrationEvents => _integrationEvents.ToList();

    public void ClearIntegrationEvents() => _integrationEvents.Clear();
}