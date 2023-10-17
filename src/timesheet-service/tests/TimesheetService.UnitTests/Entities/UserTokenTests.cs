using TimesheetService.Domain.Entities;

namespace TimesheetService.UnitTests.Entities;

public class UserTokenTests
{
    [Fact]
    public void UserToken_Ctor_Should_Be_As_Expected()
    {
        var userToken = new UserToken();
        userToken.UserTokenId.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void UserToken_UseUserToken_Should_Be_As_Expected()
    {
        var userToken = new UserToken();

        userToken.IsUsed.ShouldBeFalse();
        userToken.UsedAt.ShouldBeNull();

        var dt = DateTime.Now;

        userToken.TokenRefreshed(dt);

        userToken.IsUsed.ShouldBeTrue();
        userToken.UsedAt.ShouldNotBeNull();
    }
}