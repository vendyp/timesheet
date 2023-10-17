using TimesheetService.Shared.Abstractions.Entities;

namespace TimesheetService.Domain.Entities;

public sealed class User : BaseEntity, IEntity
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = null!;
    public string NormalizedUsername { get; set; } = null!;
    public string? Salt { get; set; }
    public string? Password { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public ICollection<UserToken> UserTokens { get; set; } = new HashSet<UserToken>();
    public ICollection<UserDevice> UserDevices { get; set; } = new HashSet<UserDevice>();

    public void UpdatePassword(string salt, string password)
    {
        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentNullException(nameof(salt));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        Salt = salt;
        Password = password;
    }
}