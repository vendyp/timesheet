using Microsoft.Extensions.DependencyInjection;
using Vendyp.Timesheet.Shared.Abstractions.Databases;

namespace Vendyp.Timesheet.Shared.Infrastructure.Initializer;

public static class ServiceCollection
{
    public static void AddApplicationInitializer(this IServiceCollection services)
    {
        var initializerOptions = services.GetOptions<InitializerOptions>("initializer");

        if (initializerOptions.Enabled)
            services.AddHostedService<ApplicationInitializer>();
    }

    public static void AddInitializer<T>(this IServiceCollection services) where T : class, IInitializer
    {
        services.AddTransient<IInitializer, T>();
    }
}