using Moq;
using TimesheetService.Domain.Entities;
using TimesheetService.Infrastructure.Services;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Mapping;

namespace TimesheetService.UnitTests.Mapping;

public class CreateUserRequestTests
{
    [Fact]
    public void CreateUserRequest_Should_MapTo_User_Correctly()
    {
        var request = new CreateUserRequest
        {
            Username = "admin   ",
            Password = "Qwerty@1234",
            Fullname = "Admin Test    ",
            EmailAddress = "admin@test.com   ",
            RoleId = Guid.Empty
        };

        string salt = "salt";
        string hashResult = "hash";
        var mock = new Mock<ISha512>();
        mock.Setup(e => e.Hash(It.IsAny<string>()))
            .Returns(hashResult);
        var salter = new Salter(mock.Object);

        var expectedUser = new User();
        expectedUser.Username = request.Username.Trim();
        expectedUser.NormalizedUsername = request.Username.Trim().ToUpper();
        expectedUser.FullName = request.Fullname.Trim();
        expectedUser.Email = request.EmailAddress.Trim();
        expectedUser.Salt = salt;
        expectedUser.Password = hashResult;
        expectedUser.Email = request.EmailAddress.Trim();
        expectedUser.UserRoles.Add(new UserRole { RoleId = request.RoleId.Value });

        var resultUser = request.ToUser(salt, salter);
        
        //user id is obviously different, then set it same for make it equivalent
        resultUser.UserId = expectedUser.UserId;
        
        resultUser.ShouldBeEquivalentTo(expectedUser);
    }
}