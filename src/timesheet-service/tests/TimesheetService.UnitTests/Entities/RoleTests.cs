using TimesheetService.Domain.Entities;

namespace TimesheetService.UnitTests.Entities;

public class RoleTests
{
    [Fact]
    public void Role_Ctor_Should_Be_As_Expected()
    {
        var role = new Role();
        role.RoleId.ShouldNotBe(Guid.Empty);
        role.RoleScopes.ShouldNotBeNull();
    }
}