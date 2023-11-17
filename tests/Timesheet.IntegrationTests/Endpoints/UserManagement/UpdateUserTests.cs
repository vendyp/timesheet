using Timesheet.Core.Abstractions;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Timesheet.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class UpdateUserTests
{
    private readonly UserManagementFixture _fixture;

    public UpdateUserTests(UserManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task UpdateUser_Given_CorrectRequest_ShouldReturn_Ok()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.GetByIdAsync(Guid.Empty, CancellationToken.None);
        user!.FullName.ShouldBe("Super Administrator"); // default value

        var updateUser = new UpdateUser(scope.ServiceProvider.GetRequiredService<IDbContext>());

        const string newFullName = "Test1234";
        // Act
        var result = await updateUser.HandleAsync(new UpdateUserRequest
        {
            UserId = Guid.Empty,
            UpdateUserRequestPayload = new UpdateUserRequestPayload() { Fullname = newFullName }
        });

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));
        user = await userService.GetByIdAsync(Guid.Empty, CancellationToken.None);
        user!.FullName.ShouldBe(newFullName);
    }
}