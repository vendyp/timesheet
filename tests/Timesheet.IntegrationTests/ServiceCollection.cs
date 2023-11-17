using Timesheet.Core.Abstractions;
using Timesheet.Infrastructure.Services;
using Timesheet.IntegrationTests.Helpers;
using Timesheet.Shared.Abstractions.Clock;
using Timesheet.Shared.Abstractions.Contexts;
using Timesheet.Shared.Abstractions.Encryption;
using Timesheet.Shared.Abstractions.Files;
using Timesheet.Shared.Infrastructure.Cache;
using Timesheet.Shared.Infrastructure.Clock;
using Timesheet.Shared.Infrastructure.Encryption;
using Timesheet.Shared.Infrastructure.Serialization;
using Timesheet.WebApi.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthManager = Timesheet.IntegrationTests.Helpers.AuthManager;

namespace Timesheet.IntegrationTests;

public static class ServiceCollection
{
    public static void AddDefaultInjectedServices(this IServiceCollection services)
    {
        services.AddScoped<IContext>(_ => new Context(Guid.Empty));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddSingleton<IFileService, FileSystemServiceMock>();
        services.AddInternalMemoryCache();
        services.AddJsonSerialization();
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<ISalter, Salter>();
        services.AddEncryption();
        services.AddSingleton(new ClockOptions());
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}