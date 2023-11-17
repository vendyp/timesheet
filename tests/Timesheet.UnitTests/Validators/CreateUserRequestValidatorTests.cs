using Timesheet.WebApi.Contracts.Requests;
using Xunit.Abstractions;

namespace Timesheet.UnitTests.Validators;

public class CreateUserRequestValidatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CreateUserRequestValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> GetInvalidRequests =>
        new List<object[]>
        {
            //all null
            new object[]
            {
                new CreateUserRequest
                {
                    Username = null,
                    Password = null,
                    Fullname = null,
                    RoleId = null,
                    EmailAddress = null,
                }
            },

            //invalid email
            new object[]
            {
                new CreateUserRequest
                {
                    Username = "admin",
                    Password = "Qwerty@1234",
                    Fullname = "Administrator",
                    RoleId = Guid.NewGuid(),
                    EmailAddress = "admin",
                }
            },

            //Fullname not ascii
            new object[]
            {
                new CreateUserRequest
                {
                    Username = "admin",
                    Password = "Qwerty@1234",
                    Fullname = "管理测试",
                    RoleId = Guid.NewGuid(),
                    EmailAddress = "admin@test.com",
                }
            },

            //Fullname not ascii (2)
            new object[]
            {
                new CreateUserRequest
                {
                    Username = "admin",
                    Password = "Qwerty@1234",
                    Fullname = "管理者テスト",
                    RoleId = Guid.NewGuid(),
                    EmailAddress = "admin@test.com",
                }
            },

            //Fullname not ascii (2)
            new object[]
            {
                new CreateUserRequest
                {
                    Username = "admin",
                    Password = "Qwerty@1234",
                    Fullname = "Административный тест",
                    RoleId = Guid.NewGuid(),
                    EmailAddress = "admin@test.com",
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void CreateUserRequestValidator_IsValid_Should_Be_False(CreateUserRequest request)
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeFalse();
    }


    [Fact]
    public void CreateUserRequestValidator_IsValid_Should_Be_True()
    {
        var request = new CreateUserRequest
        {
            Username = "admin  ",
            Password = "Qwerty@1234",
            Fullname = "Super Administrator   ",
            RoleId = Guid.Empty,
            EmailAddress = "admin@test.com   "
        };

        var validator = new CreateUserRequestValidator();
        var validationResult = validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            foreach (var item in validationResult.Errors)
            {
                var msg = $"Prop: {item.PropertyName}, Msg: {item.ErrorMessage}";
                _testOutputHelper.WriteLine(msg);
            }
        }

        validationResult.IsValid.ShouldBeTrue();
    }
}