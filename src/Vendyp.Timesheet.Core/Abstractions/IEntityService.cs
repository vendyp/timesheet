using System.Linq.Expressions;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Core.Abstractions;

/// <summary>
/// Represents an entity service that provides operations for managing entities of type T.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IEntityService<T> where T : BaseEntity
{
    /// <summary>
    /// Returns the base query for querying the data of type T.
    /// </summary>
    /// <typeparam name="T">The type of data.</typeparam>
    /// <returns>The base query of type IQueryable.</returns>
    IQueryable<T> GetBaseQuery();

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity matching the unique identifier, or null if no entity is found.</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be created.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created entity, or null if the entity could not be created.</returns>
    Task<T?> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous deletion operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single entity from the database using the specified predicate expression
    /// and returns the result of applying the projection expression on the retrieved entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
    /// <typeparam name="T">The type of the result after applying the projection.</typeparam>
    /// <param name="predicate">
    /// A predicate expression used to filter the entities in the database and identify
    /// the entity to retrieve.
    /// </param>
    /// <param name="projection">
    /// A projection expression applied on the retrieved entity to transform it to the desired result.
    /// </param>
    /// <param name="cancellationToken">
    /// Optional. A cancellation token that can be used to cancel the retrieval operation.
    /// </param>
    /// <returns>
    /// A task containing the retrieved entity if found, or null if no entity is found that satisfies
    /// the specified predicate expression.
    /// </returns>
    Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> predicate,
        Expression<Func<T, T>> projection,
        CancellationToken cancellationToken = default);
}