using Timesheet.Infrastructure;
using Timesheet.Shared.Infrastructure.Cache;
using Timesheet.Shared.Infrastructure.Contexts;
using Timesheet.Shared.Infrastructure.Logging;
using Timesheet.Shared.Infrastructure.Serialization.SystemTextJson;
using Timesheet.Shared.Infrastructure.Api;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Host.UseLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthManager, AuthManager>();
builder.Services.AddSwaggerGen2();
builder.Services.AddAuth();
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddCustomApiBehavior();

//builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddInternalMemoryCache();

builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(
            new CustomRouteToken(
                "namespace",
                c => c.ControllerType.Namespace?.Split('.').Last()
            ));
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    })
    .AddJsonOptions(options =>
    {
        //remove based from discussion
        //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
    .AddCheck<ApplicationHealthCheck>("application")
    .AddCheck<DatabaseHealthCheck>("database");

var app = builder.Build();

if (!app.Environment.IsProduction())
    app.UseSwaggerGenAndReDoc();

app.UseExceptionHandler();
app.UseMiddleware<MethodNotAllowedMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("cors");
app.UseAuth();
app.UseContext();
app.UseLogging();
app.UseRouting();
app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    Predicate = _ => true,
    AllowCachingResponses = true,
    ResponseWriter = HealthCheckResponseWriter.WriteResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});
app.UseAuthorization();
app.MapControllers();
app.Run();