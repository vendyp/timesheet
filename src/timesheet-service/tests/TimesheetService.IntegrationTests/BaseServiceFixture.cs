﻿using TimesheetService.Domain;
using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Extensions;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TimesheetService.Persistence.SqlServer;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests;

public abstract class BaseServiceFixture : IAsyncLifetime
{
    protected readonly PostgreSqlContainer DbContainer;
    protected Microsoft.Extensions.DependencyInjection.ServiceCollection Services { get; set; } = null!;

    protected BaseServiceFixture(string name)
    {
        DbContainer = new PostgreSqlBuilder()
            .WithDatabase($"{name}db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        Services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
    }

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();

        Services.AddDefaultInjectedServices();
        Services.AddDbContext<SqlServerDbContext>(x =>
            x.UseNpgsql(DbContainer.GetConnectionString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        Services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());

        var provider = Services.BuildServiceProvider();

        using (var scope = provider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync();
            var rng = scope.ServiceProvider.GetRequiredService<IRng>();
            var salter = scope.ServiceProvider.GetRequiredService<ISalter>();
            var clock = scope.ServiceProvider.GetRequiredService<IClock>();
            dbContext.Insert(new Role
            {
                RoleId = RoleExtensions.SuperAdministratorId,
                Name = RoleExtensions.SuperAdministratorName,
                Code = RoleExtensions.Slug(RoleExtensions.SuperAdministratorId, RoleExtensions.SuperAdministratorName),
                Description = "Master role"
            });
            dbContext.Insert(DefaultUser.SuperAdministrator(rng, salter, clock));
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }

    /// <summary>
    /// this method if for construct your test fixture (services or seeding data)
    /// </summary>
    public abstract void ConstructFixture();

    public abstract void SetOutput(ITestOutputHelper outputHelper);

    public abstract ServiceProvider ServiceProvider { get; set; }
    public abstract ITestOutputHelper? OutputHelper { get; set; }
}