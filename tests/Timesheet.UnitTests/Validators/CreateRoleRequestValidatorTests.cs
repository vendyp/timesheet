using Timesheet.WebApi.Contracts.Requests;

namespace Timesheet.UnitTests.Validators;

public class CreateRoleRequestValidatorTests
{
    public static IEnumerable<object[]> GetInvalidRequests =>
        new List<object[]>
        {
            //all null
            new object[]
            {
                new CreateRoleRequest
                {
                    Name = null,
                    Description = null,
                    Scopes = null
                }
            },

            new object[]
            {
                new CreateRoleRequest
                {
                    Name = new string('a', 257),
                    Description = null,
                    Scopes = new List<string>
                    {
                        "test1"
                    }
                }
            },

            new object[]
            {
                new CreateRoleRequest
                {
                    Name = "管理员角色",
                    Description = null,
                    Scopes = new List<string>
                    {
                        "test1"
                    }
                }
            },

            new object[]
            {
                new CreateRoleRequest
                {
                    Name = "User",
                    Description = null,
                    Scopes = new List<string>
                    {
                        "test1",
                        new('a', 101)
                    }
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void CreateRoleRequestValidator_IsValid_Should_Be_False(CreateRoleRequest request)
    {
        var validator = new CreateRoleRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeFalse();
    }


    [Fact]
    public void CreateRoleRequestValidator_IsValid_Should_Be_True()
    {
        var request = new CreateRoleRequest
        {
            Name = "User",
            Description = "This is just a plain user",
            Scopes = new List<string>()
            {
                "lorep",
                "ipsum",
                "dolor"
            }
        };

        var validator = new CreateRoleRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeTrue();
    }
}