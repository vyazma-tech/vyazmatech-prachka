using Microsoft.AspNetCore.OutputCaching;
using Presentation.WebAPI.Configuration;

namespace Presentation.WebAPI.Policies;

public class QueueCachePolicy : IOutputCachePolicy
{
    private readonly RedisCacheConfiguration _cacheConfiguration;

    public QueueCachePolicy(RedisCacheConfiguration cacheConfiguration)
    {
        _cacheConfiguration = cacheConfiguration;
    }

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.AllowLocking = true;

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
