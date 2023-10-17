using TimesheetService.Domain.Entities;

namespace TimesheetService.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void User_Ctor_Should_Do_As_Expected()
    {
        var entity = new User();
        entity.UserId.ShouldNotBe(Guid.Empty);
        entity.UserRoles.ShouldNotBeNull();
        entity.UserTokens.ShouldNotBeNull();
        entity.UserDevices.ShouldNotBeNull();
    }

    [Fact]
    public void User_UpdatePassword_Should_Do_As_Expected()
    {
        const string salt = "salt";
        const string password = "password";

        var entity = new User();
        entity.Salt.ShouldBeNullOrWhiteSpace();
        entity.Password.ShouldBeNullOrWhiteSpace();

        entity.UpdatePassword(salt, password);
        
        entity.Salt.ShouldBe(salt);
        entity.Password.ShouldBe(password);
    }
}