using Microsoft.AspNetCore.OutputCaching;

namespace VyazmaTech.Prachka.Infrastructure.Caching;

public class QueueCachePolicy : IOutputCachePolicy
{
    private readonly RedisCacheConfiguration _cacheConfiguration;

    public QueueCachePolicy(RedisCacheConfiguration cacheConfiguration)
    {
        _cacheConfiguration = cacheConfiguration;
    }

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        context.EnableOutputCaching = false;
        context.AllowCacheLookup = false;
        context.AllowCacheStorage = false;
        context.AllowLocking = false;

        context.CacheVaryByRules.QueryKeys = "*";
        context.ResponseExpirationTimeSpan = TimeSpan.FromMinutes(_cacheConfiguration.CacheExpirationInMinutes);
        context.Tags.Add("queues");

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }
}