using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Timesheet.Shared.Infrastructure.Contexts;

public static class ServiceCollection
{
    public static void AddContext(this IServiceCollection services)
    {
        services.AddSingleton<ContextAccessor>();
        services.AddTransient(sp => sp.GetRequiredService<ContextAccessor>().Context!);
    }

    public static void UseContext(this IApplicationBuilder app)
    {
        app.Use((ctx, next) =>
        {
            ctx.RequestServices.GetRequiredService<ContextAccessor>().Context = new Context(ctx);

            return next();
        });
    }
}