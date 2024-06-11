using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace VyazmaTech.Prachka.Infrastructure.Caching;

public class RedisOutputCacheStore : IOutputCacheStore
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisOutputCacheStore(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tag);
        IDatabase database = _connectionMultiplexer.GetDatabase();

        RedisValue[] cachedKeys = await database.SetMembersAsync(tag);

        RedisKey[] keys = cachedKeys
            .Select(x => (RedisKey)x.ToString())
            .Concat(new[] { (RedisKey)tag })
            .ToArray();

        await database.KeyDeleteAsync(keys);
    }

    public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        IDatabase database = _connectionMultiplexer.GetDatabase();
        return await database.StringGetAsync(key);
    }

    public async ValueTask SetAsync(
        string key,
        byte[] value,
        string[]? tags,
        TimeSpan validFor,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        if (cancellationToken.IsCancellationRequested)
            return;

        IDatabase database = _connectionMultiplexer.GetDatabase();

        IAsyncEnumerable<string> asyncTags = tags?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<string>();
        IAsyncEnumerable<Task<bool>> tasks = GetTagsKey(database, key, asyncTags, cancellationToken);
        await Task.WhenAll(await tasks.ToListAsync(cancellationToken: cancellationToken));

        await database.StringSetAsync(key, value, validFor);
    }

    private async IAsyncEnumerable<Task<bool>> GetTagsKey(
        IDatabase database,
        string key,
        IAsyncEnumerable<string> tags,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (string tag in tags.WithCancellation(cancellationToken))
        {
            yield return database.SetAddAsync(tag, key);
        }
    }
}