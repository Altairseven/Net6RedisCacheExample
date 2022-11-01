using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;
using Api.Configuration;

namespace Api.Services;

public class CacheService : ICacheService {

    private readonly CacheSettings _settings;

    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;


    public CacheService(IConnectionMultiplexer redis, CacheSettings settings) {
        _redis = redis;
        _settings = settings;
        _cache = redis.GetDatabase();
    }

    public bool IsEnabled => _settings.Enabled;

    public void Write<T>(string key, string subKey, T data) {
        double time = _settings.DefaultLongevity;
        
        _cache.HashSet(key, subKey, JsonSerializer.Serialize(data));
        _cache.KeyExpire(key, TimeSpan.FromMilliseconds(time));
        //_cache.HashSet(key, $"{subKey}-exp", TimeSpan.FromMilliseconds(time).ToString());

    }

    public void Write<T>(string key, string subKey, T data, TimeSpan lifespan) {
        _cache.HashSet(key, subKey, JsonSerializer.Serialize(data));
        _cache.KeyExpire(key, lifespan);
        //_cache.HashSet(key, $"{subKey}-exp", lifespan.ToString());
    }

    public async Task WriteAsync<T>(string key, string subKey, T data) {
        double time = _settings.DefaultLongevity;

        await _cache.HashSetAsync(key, subKey, JsonSerializer.Serialize(data));
        await _cache.KeyExpireAsync(key, TimeSpan.FromMilliseconds(time));
        //await _cache.HashSetAsync(key, $"{subKey}-exp", TimeSpan.FromMilliseconds(time).ToString());

    }

    public async Task WriteAsync<T>(string key, string subKey, T data, TimeSpan lifespan) {
        await _cache.HashSetAsync(key, subKey, JsonSerializer.Serialize(data));
        await _cache.KeyExpireAsync(key, lifespan);
        //await _cache.HashSetAsync(key, $"{subKey}-exp", lifespan.ToString());
    }

    public T? Get<T>(string key, string subKey) {
        var cache = _cache.HashGet(key, subKey);
        if (!cache.HasValue) return default;

        var parsed = JsonSerializer.Deserialize<T>(cache);
        return parsed;
    }

    public async Task<T?> GetAsync<T>(string key, string subKey) {
        var cache = await _cache.HashGetAsync(key, subKey);
        if (!cache.HasValue) return default;
        var parsed = JsonSerializer.Deserialize<T>(cache);
        return parsed;
    }

    public bool IsCached(string key, string subkey) {
        var cache = _cache.HashExists(key, subkey);
        return (cache);
    }

    public void RemoveFromCache(string key, string? subKey = null) {
        if (subKey == null) {
            _cache.HashKeys(key).ToList().ForEach(x => _cache.HashDelete(key, x));
            return;
        }
        _cache.HashDelete(key, subKey);
    }

    public async Task RemoveFromCacheAsync(string key, string? subKey = null) {
        if (subKey == null) {
            var lista = await _cache.HashKeysAsync(key);
            foreach (var x in lista) {
                await _cache.HashDeleteAsync(key, x);
            }
            return;
        }
        await _cache.HashDeleteAsync(key, subKey);
    }
}
