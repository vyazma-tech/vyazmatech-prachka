namespace Infrastructure.Authentication.Configuration;

public class IdentityPostgresConnectionConfiguration
{
    public const string SectionKey = nameof(IdentityPostgresConnectionConfiguration);
    public string Host { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Database { get; set; } = default!;
    public int Port { get; set; } = default!;

    public string ToConnectionString()
        => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
}