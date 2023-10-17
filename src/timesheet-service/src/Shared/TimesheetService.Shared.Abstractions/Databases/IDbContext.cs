using Microsoft.EntityFrameworkCore;

namespace TimesheetService.Shared.Abstractions.Databases;

/// <summary>
/// Represents the database context interface.
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Gets the database set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The database set for the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity;

    /// <summary>
    /// Inserts the specified entity into the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be inserted into the database.</param>
    void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Attach entity to be tracked
    /// </summary>
    /// <param name="entity">The entity to be tracked into the database.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    void AttachEntity<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Removes the specified entity from the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be removed from the database.</param>
    void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Release memory of db context
    /// </summary>
    void DetachEntities();

    /// <summary>
    /// Execute raw query with result
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult?> ExecuteRawQueryWithResultAsync<TResult>(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute raw query with no result, dedicated for maybe store procedure without result
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ExecuteRawQueryWithNoResultAsync(string query, CancellationToken cancellationToken = default);
}