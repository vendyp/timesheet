using System.Runtime.CompilerServices;
using Timesheet.Core;
using Timesheet.Core.Abstractions;
using Timesheet.Infrastructure.Services;
using Timesheet.Persistence.Postgres;
using Timesheet.Shared.Abstractions.Encryption;
using Timesheet.Shared.Infrastructure;
using Timesheet.Shared.Infrastructure.Api;
using Timesheet.Shared.Infrastructure.Clock;
using Timesheet.Shared.Infrastructure.Contexts;
using Timesheet.Shared.Infrastructure.Encryption;
using Timesheet.Shared.Infrastructure.Files.FileSystems;
using Timesheet.Shared.Infrastructure.Initializer;
using Timesheet.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Timesheet.UnitTests")]

namespace Timesheet.Infrastructure;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
        services.AddSingleton<ISalter, Salter>();

        //use one of these
        //services.AddSqlServerDbContext(configuration, "sqlserver");
        services.AddPostgresDbContext(configuration, "postgres");

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();
        services.AddCorsPolicy();

        //if use azure blob service
        //make sure app setting "azureBlobService":"" is filled
        //services.AddSingleton<IAzureBlobService, AzureBlobService>();

        services.AddInitializer<CoreInitializer>();
        services.AddApplicationInitializer();
    }
}