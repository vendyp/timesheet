using Microsoft.Extensions.DependencyInjection;
using Vendyp.Timesheet.Shared.Abstractions.Serialization;
using Vendyp.Timesheet.Shared.Infrastructure.Serialization.SystemTextJson;

namespace Vendyp.Timesheet.Shared.Infrastructure.Serialization;

public static class ServiceCollection
{
    public static void AddJsonSerialization(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}