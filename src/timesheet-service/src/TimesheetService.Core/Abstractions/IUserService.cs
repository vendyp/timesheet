using TimesheetService.Domain.Entities;

namespace TimesheetService.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IUserService : IEntityService<User>
{
    /// <summary>
    /// Get user by username.
    /// </summary>
    /// <param name="username">A username as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User">User</see></returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Check is username exist
    /// </summary>
    /// <param name="username">A username as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>bool</returns>
    Task<bool> IsUserExistAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Verify password
    /// </summary>
    /// <param name="currentPassword">Current user`s password, should be pass from <see cref="User.Password">User.Password</see></param>
    /// <param name="salt">Current user`s salt, should be pass from <see cref="User.Salt">User.Salt</see></param>
    /// <param name="password">From parameter request</param>
    /// <returns></returns>
    bool VerifyPassword(string currentPassword, string salt, string password);
}