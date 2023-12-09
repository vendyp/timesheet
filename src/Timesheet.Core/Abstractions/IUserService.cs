using Timesheet.Domain.Entities;

namespace Timesheet.Core.Abstractions;

/// <summary>
/// Represents a service for managing user entities.
/// </summary>
public interface IUserService : IEntityService<User>
{
    /// <summary>
    /// Retrieves a user by their username asynchronously.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the retrieved user,
    /// or <see langword="null"/> if no user with the given username is found.</returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user with the specified username exists asynchronously.
    /// </summary>
    /// <param name="username">The username of the user to check.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>Returns a task that represents the asynchronous operation. The task result is true if the user exists; otherwise, false.</returns>
    Task<bool> IsUserExistAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies if the provided password matches the current password
    /// after applying the salt.
    /// </summary>
    /// <param name="currentPassword">The current password to compare against.</param>
    /// <param name="salt">The salt used to modify the password.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>
    /// Returns true if the password is verified, false otherwise.
    /// </returns>
    bool VerifyPassword(string currentPassword, string salt, string password);
}