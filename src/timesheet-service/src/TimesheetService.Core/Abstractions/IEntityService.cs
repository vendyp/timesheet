using TimesheetService.Shared.Abstractions.Entities;

namespace TimesheetService.Core.Abstractions;

public interface IEntityService<T> where T : BaseEntity
{
    IQueryable<T> GetBaseQuery();
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<T?> CreateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}