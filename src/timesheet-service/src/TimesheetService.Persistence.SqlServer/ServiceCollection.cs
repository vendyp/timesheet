using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Infrastructure.Initializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TimesheetService.Persistence.SqlServer;

public static class ServiceCollection
{
    public const string DefaultConfigName = "DefaultConnection";

    public static void AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration,
        string connStringName = DefaultConfigName)
    {
        services.AddDbContext<SqlServerDbContext>(
            x => x.UseSqlServer(configuration.GetConnectionString(connStringName))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());
        
        services.AddInitializer<AutoMigrationService>();
    }

    public static void AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration,
        Action<SqlServerDbContextOptionsBuilder>? action,
        string connStringName = DefaultConfigName)
    {
        services.AddDbContext<SqlServerDbContext>(x =>
            x.UseSqlServer(configuration.GetConnectionString(connStringName), action)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());
        
        services.AddInitializer<AutoMigrationService>();
    }
}