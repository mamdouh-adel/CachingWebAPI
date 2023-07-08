using System.Text.Json;
using StackExchange.Redis;

namespace CachingWebAPI.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDB;

    public CacheService(IConfiguration configuration)
    {
        string redisConnection = configuration.GetValue<string>("RedisConnection");
        var redis = ConnectionMultiplexer.Connect(redisConnection);
        _cacheDB = redis.GetDatabase();
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        string? valueAsString = await _cacheDB.StringGetAsync(key);
        if (string.IsNullOrWhiteSpace(valueAsString))
            return default;

        return JsonSerializer.Deserialize<T>(valueAsString);
    }

    public async Task<bool> RemoveDataAsync(string key)
    {
        bool isExists = await _cacheDB.KeyExistsAsync(key);
        if (!isExists)
            return false;

        return await _cacheDB.KeyDeleteAsync(key);
    }

    public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
    {
        bool isExists = await _cacheDB.KeyExistsAsync(key);
        if (isExists)
            return false;

        string? valueAsString = JsonSerializer.Serialize(value);

        TimeSpan expiryDateTime = expirationTime.Subtract(DateTime.Now);

        return await _cacheDB.StringSetAsync(key, valueAsString, expiryDateTime);
    }
}
