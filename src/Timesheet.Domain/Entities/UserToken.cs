using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Domain.Entities;

public sealed class UserToken : BaseEntity, IEntity
{
    public Guid UserTokenId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Expiration of refresh token key
    /// </summary>
    public DateTime ExpiryAt { get; set; }

    /// <summary>
    /// Flag that will use to identify refresh token key is already used
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// When that refresh token key successfully used
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// Update IsUsed and UsedAt properties.
    /// </summary>
    /// <param name="dt">DateTime</param>
    public void TokenRefreshed(DateTime dt)
    {
        IsUsed = true;
        UsedAt = dt;
    }
}