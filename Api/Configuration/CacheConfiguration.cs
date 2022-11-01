using Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Api.Configuration;

public static class CacheConfiguration {

    public static void ConfigureDistributedCache(this IServiceCollection services, ConfigurationManager configuration) {

        var cacheSettings = new CacheSettings();
        configuration.GetSection("Redis").Bind(cacheSettings);
        services.AddSingleton(cacheSettings);

        if (cacheSettings.Enabled) {
            //services.AddStackExchangeRedisCache(options => options.Configuration = $"{cacheSettings.Host}:{cacheSettings.Port}");

            services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect($"{cacheSettings.Host}:{cacheSettings.Port}"));

            services.AddSingleton<ICacheService, CacheService>();
        }
        else {
            services.AddSingleton<ICacheService, DummyCacheService>();
        }
    }
}