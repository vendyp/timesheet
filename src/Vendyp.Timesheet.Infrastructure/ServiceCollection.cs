using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendyp.Timesheet.Core;
using Vendyp.Timesheet.Core.Abstractions;
using Vendyp.Timesheet.Infrastructure.Services;
using Vendyp.Timesheet.Persistence.Postgres;
using Vendyp.Timesheet.Shared.Abstractions.Encryption;
using Vendyp.Timesheet.Shared.Infrastructure;
using Vendyp.Timesheet.Shared.Infrastructure.Api;
using Vendyp.Timesheet.Shared.Infrastructure.Clock;
using Vendyp.Timesheet.Shared.Infrastructure.Contexts;
using Vendyp.Timesheet.Shared.Infrastructure.Encryption;
using Vendyp.Timesheet.Shared.Infrastructure.Files.FileSystems;
using Vendyp.Timesheet.Shared.Infrastructure.Initializer;
using Vendyp.Timesheet.Shared.Infrastructure.Serialization;

[assembly: InternalsVisibleTo("Timesheet.UnitTests")]

namespace Vendyp.Timesheet.Infrastructure;

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