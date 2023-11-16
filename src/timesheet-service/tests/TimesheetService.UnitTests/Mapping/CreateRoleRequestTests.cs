using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Extensions;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Mapping;

namespace TimesheetService.UnitTests.Mapping;

public class CreateRoleRequestTests
{
    [Fact]
    public void CreateRoleRequest_Should_MapTo_Role_Correctly()
    {
        var request = new CreateRoleRequest
        {
            Name = "Dolor Ipsum    ",
            Description = "Desc    "
        };
        request.Scopes.Add("user.scope");

        var expectedResult = new Role
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim()
        };
        expectedResult.Code = RoleExtensions.Slug(expectedResult.RoleId, expectedResult.Name);

        foreach (var item in request.Scopes)
            expectedResult.RoleScopes.Add(new RoleScope
            {
                Name = item
            });

        var result = request.ToRole();

        //for checking equivalent
        result.RoleId = expectedResult.RoleId;
        result.Code = expectedResult.Code;
        foreach (var item in expectedResult.RoleScopes)
            item.RoleScopeId = Guid.Empty;
        foreach (var item in result.RoleScopes)
            item.RoleScopeId = Guid.Empty;

        expectedResult.ShouldBeEquivalentTo(result);
    }
}