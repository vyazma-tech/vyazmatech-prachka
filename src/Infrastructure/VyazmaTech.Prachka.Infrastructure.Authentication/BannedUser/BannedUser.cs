namespace VyazmaTech.Prachka.Infrastructure.Authentication.BannedUser;

public sealed class BannedUser
{
    public long Id { get; init; }

    public string User { get; set; } = string.Empty;

    public string BannedBy { get; set; } = string.Empty;

    public DateTime BannedAt { get; set; }
}