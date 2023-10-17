using TimesheetService.Shared.Abstractions.Serialization;
using TimesheetService.Shared.Infrastructure.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace TimesheetService.Shared.Infrastructure.Serialization;

public static class ServiceCollection
{
    public static void AddJsonSerialization(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}