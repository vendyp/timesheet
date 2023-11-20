using Timesheet.WebApi.Contracts.Requests;

namespace Timesheet.UnitTests.Validators;

public class EditRoleRequestValidatorTests
{
    public static IEnumerable<object[]> GetInvalidRequests =>
        new List<object[]>
        {
            //all null
            new object[]
            {
                new EditRoleRequest
                {
                    RoleId = Guid.Empty,
                    Payload = null!
                }
            },

            new object[]
            {
                new EditRoleRequest
                {
                    RoleId = Guid.Empty,
                    Payload = new EditRoleRequestPayload
                    {
                        Description = "该角色的作用是输入和删除数据",
                        Scopes = new List<string>()
                    }
                }
            },

            new object[]
            {
                new EditRoleRequest
                {
                    RoleId = Guid.Empty,
                    Payload = new EditRoleRequestPayload
                    {
                        Description = "Just for deleting data",
                        Scopes = new List<string>
                        {
                            new('a', 101)
                        }
                    }
                }
            },

            new object[]
            {
                new EditRoleRequest
                {
                    RoleId = Guid.Empty,
                    Payload = new EditRoleRequestPayload
                    {
                        Description = "Just for deleting data",
                        Scopes = new List<string>
                        {
                            "超级删除管理员"
                        }
                    }
                }
            },
        };

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void EditRoleRequestValidator_IsValid_Should_Be_False(EditRoleRequest request)
    {
        var validator = new EditRoleRequestValidator();
        var validationResult = validator.Validate(request);
        validationResult.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void EditRoleRequestValidator_IsValid_Should_Be_True()
    {
        var request = new EditRoleRequest
        {
            RoleId = Guid.NewGuid(),
            Payload = new EditRoleRequestPayload
            {
                Description = "this role is for sub administator",
                Scopes = new List<string>
                {
                    "lorep",
                    "ipsum",
                    "dolor"
                }
            }
        };

        var validator = new EditRoleRequestValidator();
        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }
}