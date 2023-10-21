using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.WebApi;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using TimesheetService.Persistence.SqlServer;
using Xunit.Abstractions;

namespace TimesheetService.FunctionalTests;

public class CustomWebApplicationFactory : WebApplicationFactory<ApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(x =>
        {
            if (Output == null)
                return;

            x.ClearProviders();
            x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(Output));
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDbContext>();
            services.RemoveAll<DbContextOptions<SqlServerDbContext>>();
            services.RemoveAll<SqlServerDbContext>();

            services.AddDbContext<SqlServerDbContext>(x =>
                x.UseSqlServer(_dbContainer.GetConnectionString())
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());
        });
    }

    public ITestOutputHelper? Output { get; set; }

    public void SetOutPut(ITestOutputHelper output)
    {
        Output = output;
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}