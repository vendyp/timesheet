namespace Vendyp.Timesheet.Shared.Abstractions.Clock;

public interface IClock
{
    DateTime CurrentDate();
    DateTime CurrentServerDate();
}