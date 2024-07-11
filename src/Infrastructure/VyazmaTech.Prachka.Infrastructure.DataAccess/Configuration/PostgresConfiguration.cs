namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;

public class PostgresConfiguration
{
    public const string SectionKey = "Infrastructure:DataAccess:PostgresConfiguration";

    public string Host { get; set; } = default!;

    public string Database { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int Port { get; set; } = default!;

    public string? ConnectionString { get; set; } = default!;

    public string ToConnectionString()
    {
        return ConnectionString is null
            ? $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};"
            : ConnectionString;
    }
}