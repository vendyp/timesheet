using TimesheetService.Shared.Abstractions.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace TimesheetService.Shared.Infrastructure.Cache;

public static class ServiceCollection
{
    public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration,
        string connectionStringName = "redis")
    {
        services.AddStackExchangeRedisCache(o =>
            o.Configuration = configuration.GetConnectionString(connectionStringName));
        services.TryAddScoped<ICache, RedisCache>();
        services.TryAddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString(connectionStringName)));
        services.TryAddScoped(ctx => ctx.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
    }

    public static void AddInternalMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.TryAddScoped<ICache, InMemoryCache>();
    }
}