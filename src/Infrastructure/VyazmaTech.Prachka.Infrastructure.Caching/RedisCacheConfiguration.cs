namespace VyazmaTech.Prachka.Infrastructure.Caching;

public class RedisCacheConfiguration
{
    public const string SectionKey = "Infrastructure:Caching";

    public string ConnectionString { get; set; } = default!;

    public double CacheExpirationInMinutes { get; set; } = default!;
}