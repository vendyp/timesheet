using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Domain.Entities;

public sealed class Option : BaseEntity, IEntity
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string? Description { get; set; }
}