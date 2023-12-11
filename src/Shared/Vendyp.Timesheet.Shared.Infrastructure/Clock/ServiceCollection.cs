using Microsoft.Extensions.DependencyInjection;
using Vendyp.Timesheet.Shared.Abstractions.Clock;

namespace Vendyp.Timesheet.Shared.Infrastructure.Clock;

public static class ServiceCollection
{
    public static void AddClock(this IServiceCollection services)
    {
        var clockOptions = services.GetOptions<ClockOptions>("clock");

        services.AddSingleton(clockOptions)
            .AddSingleton<IClock, Clock>();
    }
}