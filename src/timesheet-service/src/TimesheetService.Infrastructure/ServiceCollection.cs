using System.Runtime.CompilerServices;
using TimesheetService.Core;
using TimesheetService.Core.Abstractions;
using TimesheetService.Infrastructure.Services;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.Shared.Infrastructure;
using TimesheetService.Shared.Infrastructure.Api;
using TimesheetService.Shared.Infrastructure.Clock;
using TimesheetService.Shared.Infrastructure.Contexts;
using TimesheetService.Shared.Infrastructure.Encryption;
using TimesheetService.Shared.Infrastructure.Files.FileSystems;
using TimesheetService.Shared.Infrastructure.Initializer;
using TimesheetService.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimesheetService.Persistence.SqlServer;

[assembly: InternalsVisibleTo("TimesheetService.UnitTests")]

namespace TimesheetService.Infrastructure;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
        services.AddScoped<ITimesheetService, Services.TimesheetService>();
        services.AddSingleton<ISalter, Salter>();

        services.AddSqlServerDbContext(configuration, "sqlserver");

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();
        services.AddCorsPolicy();

        services.AddInitializer<AutoMigrationService>();
        services.AddInitializer<CoreInitializer>();
        services.AddApplicationInitializer();
    }
}