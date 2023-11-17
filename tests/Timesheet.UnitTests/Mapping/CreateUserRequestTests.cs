using Moq;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Encryption;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Mapping;

namespace Timesheet.UnitTests.Mapping;

public class CreateUserRequestTests
{
    [Fact]
    public void CreateUserRequest_Should_MapTo_User_Correctly()
    {
        var request = new CreateUserRequest
        {
            Username = "admin  ",
            Password = "Qwerty@1234",
            Fullname = "Super Administrator   ",
            RoleId = Guid.Empty,
            EmailAddress = "admin@test.com   "
        };

        string salt = "salt";
        string hashPassword = "hashed";
        var salter = new Mock<ISalter>();
        salter.Setup(e => e.Hash(salt, It.IsAny<string>()))
            .Returns(hashPassword);

        var expectedUser = new User();
        expectedUser.Username = request.Username.Trim();
        expectedUser.NormalizedUsername = expectedUser.Username.ToUpper();
        expectedUser.FullName = request.Fullname.Trim();
        expectedUser.Email = request.EmailAddress.Trim();
        expectedUser.Salt = salt;
        expectedUser.Password = hashPassword;
        expectedUser.UserRoles.Add(new UserRole
        {
            RoleId = request.RoleId.Value
        });

        var result = request.ToUser(salt, salter.Object);

        //to set equivalent
        result.UserId = expectedUser.UserId;

        result.ShouldBeEquivalentTo(expectedUser);
    }
}