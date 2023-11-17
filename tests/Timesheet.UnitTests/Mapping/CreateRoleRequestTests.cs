using Timesheet.Domain.Entities;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Mapping;

namespace Timesheet.UnitTests.Mapping;

public class CreateRoleRequestTests
{
    [Fact]
    public void CreateRoleRequest_Should_MapTo_Role_Correctly()
    {
        var request = new CreateRoleRequest
        {
            Name = "Dolor Ipsum",
            Description = "Desc"
        };
        request.Scopes.Add("user.scope");

        var expectedResult = new Role
        {
            Name = request.Name,
            Description = request.Description
        };
        foreach (var item in request.Scopes)
            expectedResult.RoleScopes.Add(new RoleScope
            {
                Name = item
            });

        var result = request.ToRole();

        result.Name.ShouldBe(expectedResult.Name);
        result.Description.ShouldBe(expectedResult.Description);
        result.RoleScopes.Count.ShouldBe(expectedResult.RoleScopes.Count);
    }
}