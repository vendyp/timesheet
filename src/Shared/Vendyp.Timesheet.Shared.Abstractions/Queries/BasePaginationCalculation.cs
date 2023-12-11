namespace Vendyp.Timesheet.Shared.Abstractions.Queries;

public abstract class BasePaginationCalculation
{
    protected BasePaginationCalculation()
    {
        Page = 1;
        Size = 10;
    }

    public int Page { get; set; }
    public int Size { get; set; }
    public int CalculateSkip() => (Page - 1) * Size;
    public string? OrderBy { get; set; }
    public string? OrderType { get; set; }
}