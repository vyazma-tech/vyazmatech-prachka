namespace Presentation.WebAPI.Configuration;

public class DbNameConfiguration
{
    public const string SectionKey = nameof(DbNameConfiguration);

    public string DatabaseName { get; set; } = default!;
}