using TimesheetService.Shared.Abstractions.Cache;
using TimesheetService.Shared.Abstractions.Serialization;
using StackExchange.Redis;

namespace TimesheetService.Shared.Infrastructure.Cache;

internal sealed class RedisCache : ICache
{
    private static readonly HashSet<Type> PrimitiveTypes = new()
    {
        typeof(string),
        typeof(char),
        typeof(int),
        typeof(long),
        typeof(Guid),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(short),
        typeof(uint),
        typeof(ulong)
    };

    private readonly IDatabase _database;
    private readonly IJsonSerializer _jsonSerializer;

    public RedisCache(IDatabase database, IJsonSerializer jsonSerializer)
    {
        _database = database;
        _jsonSerializer = jsonSerializer;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return default;
        }

        var value = await _database.StringGetAsync(key);
        return string.IsNullOrWhiteSpace(value) ? default : _jsonSerializer.Deserialize<T>(value!);
    }

    public async Task<IReadOnlyList<T>> GetManyAsync<T>(params string[] keys)
    {
        var values = new List<T>();
        if (!keys.Any())
        {
            return values;
        }

        var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
        var redisValues = await _database.StringGetAsync(redisKeys);

        values.AddRange(from redisValue in redisValues
            where redisValue.HasValue && !redisValue.IsNullOrEmpty
            select _jsonSerializer.Deserialize<T>(redisValue!)!);

        return values;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        => _database.StringSetAsync(key, _jsonSerializer.Serialize(value), expiry);

    public Task DeleteAsync(string key)
        => _database.KeyDeleteAsync(key);

    private string AsString<T>(T value)
        => (PrimitiveTypes.Contains(typeof(T)) ? value!.ToString() : _jsonSerializer.Serialize(value))!;
}