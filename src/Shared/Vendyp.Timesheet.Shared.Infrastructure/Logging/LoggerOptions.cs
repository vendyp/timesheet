using Vendyp.Timesheet.Shared.Infrastructure.Logging.Options;
using FileOptions = Vendyp.Timesheet.Shared.Infrastructure.Logging.Options.FileOptions;

namespace Vendyp.Timesheet.Shared.Infrastructure.Logging;

internal sealed class LoggerOptions
{
    public string Level { get; set; } = "Information";
    public ConsoleOptions? Console { get; set; }
    public FileOptions? File { get; set; }
    public SeqOptions? Seq { get; set; }
    public IDictionary<string, string>? Overrides { get; set; } = new Dictionary<string, string>();
    public List<string>? ExcludePaths { get; set; } = new();
    public List<string>? ExcludeProperties { get; set; } = new();
    public IDictionary<string, object>? Tags { get; set; } = new Dictionary<string, object>();
}