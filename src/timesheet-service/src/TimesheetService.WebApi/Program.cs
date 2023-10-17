using TimesheetService.Infrastructure;
using TimesheetService.Shared.Infrastructure.Cache;
using TimesheetService.Shared.Infrastructure.Contexts;
using TimesheetService.Shared.Infrastructure.Logging;
using TimesheetService.Shared.Infrastructure.Serialization.SystemTextJson;
using TimesheetService.Shared.Infrastructure.Api;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
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

builder.Services.AddInternalMemoryCache();

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = false; });
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

app.UseMiddleware<PerformanceMiddleware>();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("cors");
app.UseCustomExceptionHandler();
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