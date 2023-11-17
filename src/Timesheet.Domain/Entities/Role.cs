using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Domain.Entities;

public sealed class Role : BaseEntity
{
    public Guid RoleId { get; set; } = Guid.NewGuid();
    public bool IsDefault { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<RoleScope> RoleScopes { get; set; } = new HashSet<RoleScope>();
}