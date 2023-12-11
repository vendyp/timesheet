using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vendyp.Timesheet.Shared.Abstractions.Models;

namespace Vendyp.Timesheet.Shared.Infrastructure.Api;

public static class ServiceCollection
{
    private const string DefaultName = "cors";

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        var corsOptions = services.GetOptions<CorsOptions>("cors");

        services
            .AddSingleton(corsOptions)
            .AddCors(cors =>
            {
                var allowedHeaders = corsOptions.AllowedHeaders;
                var allowedMethods = corsOptions.AllowedMethods;
                var allowedOrigins = corsOptions.AllowedOrigins;
                var exposedHeaders = corsOptions.ExposedHeaders;
                cors.AddPolicy(DefaultName, corsBuilder =>
                {
                    var origins = allowedOrigins.ToArray();
                    if (corsOptions.AllowCredentials && origins.FirstOrDefault() != "*")
                    {
                        corsBuilder.AllowCredentials();
                    }
                    else
                    {
                        corsBuilder.DisallowCredentials();
                    }

                    corsBuilder.WithHeaders(allowedHeaders.ToArray())
                        .WithMethods(allowedMethods.ToArray())
                        .WithOrigins(origins.ToArray())
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithExposedHeaders(exposedHeaders.ToArray());
                });
            });
    }

    public static string GetUserIpAddress(this HttpContext? context)
    {
        if (context is null)
        {
            return string.Empty;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (!context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
            return ipAddress ?? string.Empty;

        var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
        if (ipAddresses.Any())
            ipAddress = ipAddresses[0];

        return ipAddress ?? string.Empty;
    }

    public static void AddCustomApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = context =>
            {
                var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                if (env.IsProduction())
                    return new BadRequestObjectResult(Error.Create("Invalid parameter"));

                return new BadRequestObjectResult(Error.Create("Invalid parameter",
                    "To developer, please check your request, this error occur while construct model binding",
                    400));
            };
        });
    }

    public static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}