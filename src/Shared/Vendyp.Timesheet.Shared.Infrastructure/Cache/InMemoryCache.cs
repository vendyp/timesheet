using Microsoft.Extensions.Caching.Memory;
using Vendyp.Timesheet.Shared.Abstractions.Cache;
using Vendyp.Timesheet.Shared.Abstractions.Serialization;

namespace Vendyp.Timesheet.Shared.Infrastructure.Cache;

public class InMemoryCache : ICache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IJsonSerializer _jsonSerializer;

    public InMemoryCache(IMemoryCache memoryCache, IJsonSerializer jsonSerializer)
    {
        _memoryCache = memoryCache;
        _jsonSerializer = jsonSerializer;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Task.FromResult<T?>(default);

        var value = _memoryCache.Get<string>(key);

        return value is null
            ? Task.FromResult<T?>(default)
            : Task.FromResult(_jsonSerializer.Deserialize<T>(value));
    }

    public Task<IReadOnlyList<T>> GetManyAsync<T>(params string[] keys)
    {
        var values = new List<T>();

        if (!keys.Any())
            return Task.FromResult<IReadOnlyList<T>>(values);

        foreach (var item in keys)
        {
            var value = _memoryCache.Get<string>(item);
            if (!string.IsNullOrWhiteSpace(value))
                values.Add(_jsonSerializer.Deserialize<T>(value)!);
        }

        return Task.FromResult<IReadOnlyList<T>>(values);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        _memoryCache.Set(key, _jsonSerializer.Serialize(value), expiry ?? TimeSpan.FromMinutes(3));
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}