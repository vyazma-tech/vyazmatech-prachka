namespace Presentation.WebAPI.Configuration;

public class RedisCacheConfiguration
{
    public const string SectionKey = nameof(RedisCacheConfiguration);
    public string ConnectionString { get; set; } = default!;
    public double CacheExpirationInMinutes { get; set; } = default!;
}