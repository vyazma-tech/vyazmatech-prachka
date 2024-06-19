namespace VyazmaTech.Prachka.Infrastructure.Authentication.Configuration;

public class PostgresConfiguration
{
    public const string SectionKey = "Infrastructure:Identity:PostgresConfiguration";

    public string Host { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Database { get; set; } = default!;

    public int Port { get; set; } = default!;

    public string ToConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
    }
}