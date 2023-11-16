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

    [Fact]
    public void Timesheet_Rejected_Should_Do_As_Expected()
    {
        var msg = "Lorep ipsum";
        var dt1 = DateTime.UtcNow;
        var dt2 = DateTime.UtcNow.AddDays(-3);

        var entity = new Timesheet();
        entity.Rejected(msg, dt1, dt2);
        entity.RejectedMessage.ShouldBe(msg);
        entity.ApprovedAt.ShouldBe(dt1);
        entity.ApprovedAtServer.ShouldBe(dt2);
    }
}