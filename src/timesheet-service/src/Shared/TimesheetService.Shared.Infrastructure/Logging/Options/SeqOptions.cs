namespace TimesheetService.Shared.Infrastructure.Logging.Options;

internal sealed class SeqOptions
{
    public bool Enabled { get; set; }
    public string Url { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}