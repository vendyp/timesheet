using Timesheet.Domain.Entities;

namespace Timesheet.UnitTests.Entities;

public class UserRoleTests
{
    [Fact]
    public void UserRole_Ctor_Should_Be_As_Expected()
    {
        var userRole = new UserRole();
        userRole.UserId.ShouldBe(Guid.Empty);
        userRole.RoleId.ShouldBe(Guid.Empty);
    }
}