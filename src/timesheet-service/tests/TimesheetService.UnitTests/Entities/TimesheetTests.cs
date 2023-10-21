using TimesheetService.Domain.Entities;

namespace TimesheetService.UnitTests.Entities;

public class TimesheetTests
{
    [Fact]
    public void Timesheet_Ctor_Should_Do_As_Expected()
    {
        var timesheet = new Timesheet();
        timesheet.TimesheetId.ShouldNotBe(Guid.Empty);
    }
}