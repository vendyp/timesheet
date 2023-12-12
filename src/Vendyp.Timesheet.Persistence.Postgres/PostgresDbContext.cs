using Microsoft.EntityFrameworkCore;
using Vendyp.Timesheet.Shared.Abstractions.Clock;
using Vendyp.Timesheet.Shared.Abstractions.Contexts;
using Vendyp.Timesheet.Shared.Abstractions.Databases;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Persistence.Postgres;

public class PostgresDbContext : DbContext, IDbContext
{
    private readonly IContext? _context;
    private readonly IClock _clock;

    public PostgresDbContext(
        DbContextOptions<PostgresDbContext> options,
        IContext? context,
        IClock clock)
        : base(options)
    {
        _context = context;
        _clock = clock;
    }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity =>
        base.Set<TEntity>();

    public void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity =>
        Set<TEntity>().Add(entity);

    public async Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void AttachEntity<TEntity>(TEntity entity)
        where TEntity : BaseEntity =>
        Attach(entity);

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity =>
        Set<TEntity>().Remove(entity);

    public void DetachEntities()
    {
        var changedEntriesCopy = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }

    public async Task<TResult?> ExecuteRawQueryWithResultAsync<TResult>(string query,
        CancellationToken cancellationToken = default)
    {
        var command = Database.GetDbConnection().CreateCommand();

        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = query;

        await Database.OpenConnectionAsync(cancellationToken);

        try
        {
            var result = (TResult?)await command.ExecuteScalarAsync(cancellationToken);

            return result;
        }
        finally
        {
            await Database.CloseConnectionAsync();
        }
    }

    public async Task ExecuteRawQueryWithNoResultAsync(string query, CancellationToken cancellationToken = default)
    {
        var command = Database.GetDbConnection().CreateCommand();

        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = query;

        await Database.OpenConnectionAsync(cancellationToken);

        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            await Database.CloseConnectionAsync();
        }
    }

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var contextExist = !(_context is null || !_context.Identity.IsAuthenticated);

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (contextExist)
                    {
                        entry.Entity.CreatedBy = _context!.Identity.Id.ToString();
                        entry.Entity.CreatedByName = _context.Identity.Username;
                    }

                    entry.Entity.CreatedAt = _clock.CurrentDate();
                    entry.Entity.CreatedAtServer = _clock.CurrentServerDate();
                    break;
                }
                case EntityState.Modified:
                    if (contextExist)
                    {
                        entry.Entity.LastUpdatedBy = _context!.Identity.Id.ToString();
                        entry.Entity.LastUpdatedByName = _context.Identity.Username;
                    }
                    else
                    {
                        if (entry.Entity.LastUpdatedBy is not null)
                        {
                            entry.Entity.LastUpdatedBy = null;
                            entry.Entity.LastUpdatedByName = null;
                        }
                    }

                    entry.Entity.LastUpdatedAt = _clock.CurrentDate();
                    entry.Entity.LastUpdatedAtServer = _clock.CurrentServerDate();

                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    throw new Exception("Deleted is not acceptable");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}