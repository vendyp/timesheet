using Timesheet.Shared.Abstractions.Databases;
using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Core.Abstractions;

public abstract class BaseEntityService<T> : IEntityService<T> where T : BaseEntity
{
    protected readonly IDbContext DbContext;

    protected BaseEntityService(IDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual IQueryable<T> GetBaseQuery()
        => DbContext.Set<T>().AsQueryable();

    public abstract Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public abstract Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

    public abstract Task<T?> CreateAsync(T entity, CancellationToken cancellationToken);

    public abstract Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}