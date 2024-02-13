namespace VyazmaTech.Prachka.Infrastructure.Authentication.Configuration;

public class TokenConfiguration
{
    public const string SectionKey = "Infrastructure:Identity:TokenConfiguration";

    public string Secret { get; init; } = string.Empty;

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public int RefreshTokenExpiresInMinutes { get; init; }

    public int AccessTokenExpiresInMinutes { get; init; }
}