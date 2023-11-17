namespace Timesheet.Shared.Abstractions.Entities;

/// <summary>
/// Base in all entities must have these.
/// </summary>
public abstract class BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? CreatedByFullName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CreatedAtServer { get; set; }

    public string? LastUpdatedBy { get; set; }
    public string? LastUpdatedByName { get; set; }
    public string? LastUpdatedByFullName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? LastUpdatedAtServer { get; set; }

    /// <summary>
    /// Default value is false.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Default value is <see cref="Enums.StatusRecord.Active">Active</see>
    /// </summary>
    public StatusRecord StatusRecord { get; set; } = StatusRecord.Active;

    public void SetStatusRecordToInActive()
    {
        StatusRecord = StatusRecord.InActive;
    }

    public void SetStatusRecordToActive()
    {
        StatusRecord = StatusRecord.Active;
    }

    public void SetToDeleted()
    {
        SetStatusRecordToInActive();
        IsDeleted = true;
    }

    public void ModifyLastUpdated(string? lastUpdateBy, string? lastUpdateByName, DateTime? lastUpdateAt)
    {
        LastUpdatedBy = lastUpdateBy;
        LastUpdatedByName = lastUpdateByName;
        LastUpdatedAt = lastUpdateAt;
    }
}