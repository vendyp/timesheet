using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Extensions;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class CreateUserTests
{
    private readonly UserManagementFixture _fixture;

    public CreateUserTests(UserManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateUser_Given_CorrectRequest_With_CorrectValue_ShouldReturn_NoContent()
    {
        // Arrange
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();

        var createUser = new CreateUser(
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            userService,
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>());

        var request = new CreateUserRequest
        {
            Username = "admin2",
            Password = "Test@12345",
            Fullname = "Super Administrator",
            RoleId = RoleExtensions.SuperAdministratorId,
            EmailAddress = "test@test.com"
        };

        // Act
        var result = await createUser.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var user = await userService.GetByUsernameAsync(request.Username, CancellationToken.None);
        user.ShouldNotBeNull();
        user.NormalizedUsername.ShouldBe(request.Username.ToUpper());
    }
}