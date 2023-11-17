using Timesheet.Shared.Abstractions.Serialization;
using Timesheet.Shared.Infrastructure.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace Timesheet.Shared.Infrastructure.Serialization;

public static class ServiceCollection
{
    public static void AddJsonSerialization(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}