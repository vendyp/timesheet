namespace TimesheetService.Shared.Infrastructure.Logging.Options;

internal sealed class FileOptions
{
    public bool Enabled { get; set; }
    public string Path { get; set; } = null!;
    public string Interval { get; set; } = null!;
}