using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Domain.Entities;

public sealed class RoleScope : BaseEntity
{
    public Guid RoleScopeId { get; set; } = Guid.NewGuid();
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedMessage { get; set; }
}