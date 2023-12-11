using Microsoft.EntityFrameworkCore;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Shared.Abstractions.Databases;

/// <summary>
/// Represents a database context.
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Gets a <see cref="DbSet{TEntity}"/> instance for the given entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>A <see cref="DbSet{TEntity}"/> instance for the given entity type.</returns>
    DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity;

    /// <summary>
    /// Inserts the specified entity into the database.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <remarks>
    /// This method inserts the specified entity into the database. The entity must derive from <see cref="BaseEntity"/>.
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity being inserted.</typeparam>
    void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Inserts a new entity asynchronously into the database.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be inserted.</param>
    /// <param name="cancellationToken">The cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Attaches an entity to the database context.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to attach</typeparam>
    /// <param name="entity">The entity to attach</param>
    /// <remarks>
    /// This method is used to attach an entity to the database context so that it can be tracked for changes and
    /// persisted to the database when changes are saved.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="entity"/> parameter is null.</exception>
    /// <seealso cref="BaseEntity"/>
    void AttachEntity<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Removes the specified entity from the system.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be removed.</param>
    /// <remarks>
    /// This method removes the specified entity from the system. The entity must inherit from the BaseEntity class.
    /// </remarks>
    void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Saves all changes made to the context asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>Returns a task representing the asynchronous operation. The task result is the number of state entries written to the underlying database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Detaches all entities from the current DbContext, removing them from the tracking mechanism.
    /// </summary>
    void DetachEntities();

    /// <summary>
    /// Executes a raw query and returns the result asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="query">The raw query to execute.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The result of the task is the nullable <typeparamref name="TResult"/>.</returns>
    Task<TResult?> ExecuteRawQueryWithResultAsync<TResult>(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a raw SQL query with no result asynchronously.
    /// </summary>
    /// <param name="query">The SQL query to be executed.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the execution.</param>
    /// <returns>A task representing the asynchronous execution of the query.</returns>
    Task ExecuteRawQueryWithNoResultAsync(string query, CancellationToken cancellationToken = default);
}