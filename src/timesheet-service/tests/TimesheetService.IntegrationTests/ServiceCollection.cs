using TimesheetService.Core.Abstractions;
using TimesheetService.Infrastructure.Services;
using TimesheetService.IntegrationTests.Helpers;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Contexts;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.Shared.Abstractions.Files;
using TimesheetService.Shared.Infrastructure.Cache;
using TimesheetService.Shared.Infrastructure.Clock;
using TimesheetService.Shared.Infrastructure.Encryption;
using TimesheetService.Shared.Infrastructure.Serialization;
using TimesheetService.WebApi.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthManager = TimesheetService.IntegrationTests.Helpers.AuthManager;

namespace TimesheetService.IntegrationTests;

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