namespace Api.Services;

public interface ICacheService {

    bool IsEnabled { get; }

    void Write<T>(string key, string subkey, T data);
    void Write<T>(string key, string subkey, T data, TimeSpan lifespan);

    Task WriteAsync<T>(string key, string subkey, T data);
    Task WriteAsync<T>(string key, string subkey, T data, TimeSpan livetime);

    T? Get<T>(string key, string subkey);
    Task<T?> GetAsync<T>(string key, string subkey);

    bool IsCached(string key, string subkey);

    void RemoveFromCache(string key, string? subkey = null);
    Task RemoveFromCacheAsync(string key, string? subkey = null);
}

