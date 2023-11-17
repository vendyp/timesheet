using Timesheet.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Timesheet.Persistence.Postgres;

public class AutoMigrationService : IInitializer
{
    private readonly PostgresDbContext _dbContext;
    private readonly ILogger<AutoMigrationService> _logger;

    public AutoMigrationService(PostgresDbContext dbContext, ILogger<AutoMigrationService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurs when database migrations");
        }
    }
}