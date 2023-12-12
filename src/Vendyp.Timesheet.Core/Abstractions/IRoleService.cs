using Vendyp.Timesheet.Domain.Entities;

namespace Vendyp.Timesheet.Core.Abstractions;

/// <summary>
/// Provides an interface for managing roles.
/// </summary>
public interface IRoleService : IEntityService<Role>
{
    /// <summary>
    /// Retrieves a role by its code asynchronously.
    /// </summary>
    /// <param name="code">The code of the role to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the retrieved role, or null if no role is found with the specified code.</returns>
    Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}