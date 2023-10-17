using TimesheetService.Shared.Abstractions.Helpers;

namespace TimesheetService.Shared.Abstractions.Queries;

public class Filter
{
    public Filter(string name)
    {
        Name = name;
        ParameterName = Name.ToCamelCase();
        Type = FilterType.Text;
    }

    public string Name { get; }

    public string ParameterName { get; set; }

    /// <summary>
    /// Default value is <see cref="FilterType.Text"/>
    /// </summary>
    public FilterType Type { get; set; }

    /// <summary>
    /// Source endpoints
    /// </summary>
    public string? Source { get; set; }
}