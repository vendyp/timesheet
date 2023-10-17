using TimesheetService.Domain.Entities;

namespace TimesheetService.UnitTests.Entities;

public class UserDeviceTests
{
    [Fact]
    public void UserDevice_Ctor_Should_Be_As_Expected()
    {
        var userDevice = new UserDevice();
        userDevice.UserDeviceId.ShouldNotBe(Guid.Empty);
    }
}