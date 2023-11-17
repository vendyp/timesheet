using Timesheet.Domain.Entities;
using Timesheet.Domain.Extensions;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Endpoints.RoleManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Timesheet.IntegrationTests.Endpoints.RoleManagement;

[Collection(nameof(RoleManagementFixture))]
public class CreateRoleTests
{
    private readonly RoleManagementFixture _fixture;

    public CreateRoleTests(RoleManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateRole_Given_CorrectRequest_With_RoleAlreadyExists_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var createUser = new CreateRole(
            scope.ServiceProvider.GetRequiredService<IDbContext>());

        var request = new CreateRoleRequest
        {
            Name = RoleExtensions.SuperAdministratorName
        };

        // Act
        var result = await createUser.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task CreateRole_Given_CorrectRequest_ShouldReturn_NoContent()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        var createUser = new CreateRole(dbContext);

        var request = new CreateRoleRequest
        {
            Name = "Staff"
        };

        // Act
        var result = await createUser.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var role = await dbContext.Set<Role>().Where(e => e.Name == request.Name)
            .FirstOrDefaultAsync(CancellationToken.None);
        role.ShouldNotBeNull();
    }
}