namespace Timesheet.Shared.Abstractions.Queries;

public class QueryOption
{
    public QueryOption()
    {
        Columns = new List<Column>();
        Filters = new List<Filter>();
        EnableGlobalSearch = true;
        OrderValues = new List<string>
        {
            "asc",
            "desc"
        };
    }

    public List<string> OrderValues { get; }
    public List<Column> Columns { get; }
    public List<Filter> Filters { get; }
    public bool EnableGlobalSearch { get; set; }
}