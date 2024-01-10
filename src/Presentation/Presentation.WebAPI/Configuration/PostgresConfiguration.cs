namespace Presentation.WebAPI.Configuration;

public class PostgresConfiguration
{
    public const string SectionKey = nameof(PostgresConfiguration);
    public string Host { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int Port { get; set; } = default!;

    public string ToConnectionString(string databaseName)
        => $"Host={Host};Port={Port};Database={databaseName};Username={Username};Password={Password};";
}