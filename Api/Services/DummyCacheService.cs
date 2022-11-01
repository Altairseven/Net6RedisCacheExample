
namespace Api.Services;

public class DummyCacheService : ICacheService {

    public bool IsEnabled => false;

    public void Write<T>(string key, string subKey, T data) {

    }

    public void Write<T>(string key, string subKey, T data, TimeSpan livetime) {

    }

    public Task WriteAsync<T>(string key, string subKey, T data) {
        return Task.CompletedTask;
    }

    public Task WriteAsync<T>(string key, string subKey, T data, TimeSpan livetime) {
        return Task.CompletedTask;
    }

    public T? Get<T>(string key, string subKey) {
        return default;
    }

    public async Task<T?> GetAsync<T>(string key, string subKey) {
        await Task.Delay(0);
        return default;
    }

    public bool IsCached(string key, string subKey) {
        return false;
    }

    public void RemoveFromCache(string key, string? subKey = null) {

    }

    public Task RemoveFromCacheAsync(string key, string? subKey = null) {
        return Task.CompletedTask;
    }
}