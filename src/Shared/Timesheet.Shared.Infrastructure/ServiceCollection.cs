using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Timesheet.Shared.Infrastructure;

public static class ServiceCollection
{
    /// <summary>
    /// This method to change if you want to change
    /// </summary>
    /// <param name="services"></param>
    public static void AddSharedInfrastructure(this IServiceCollection services)
    {
        var appOptions = services.GetOptions<AppOptions>("app");
        services.AddSingleton(appOptions);
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }

    public static bool IsNullOrEmpty(this string? s)
        => string.IsNullOrEmpty(s);

    public static bool IsNullOrWhiteSpace(this string? s)
        => string.IsNullOrWhiteSpace(s);

    public static bool IsNotNullOrEmpty(this string? s)
        => !string.IsNullOrEmpty(s);

    public static bool IsNotNullOrWhiteSpace(this string? s)
        => !string.IsNullOrWhiteSpace(s);
}