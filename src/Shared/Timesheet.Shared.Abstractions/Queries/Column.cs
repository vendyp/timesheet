using Timesheet.Shared.Abstractions.Helpers;

namespace Timesheet.Shared.Abstractions.Queries;

public class Column
{
    public Column(string name)
    {
        Name = name;
        DataParameter = Name.ToCamelCase();
        OrderParameter = Name;
        EnableOrder = false;
    }

    /// <summary>
    /// Name of the column
    /// </summary>
    public string Name { get; }

    public string DataParameter { get; }
    
    /// <summary>
    /// name of parameter used for order
    /// </summary>
    public string OrderParameter { get; set; }

    /// <summary>
    /// Default value is false
    /// </summary>
    public bool EnableOrder { get; set; }
}