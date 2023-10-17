using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Services;

[Collection(nameof(UserServiceFixture))]
public class UserServiceTests
{
    private readonly UserServiceFixture _fixture;

    public UserServiceTests(UserServiceFixture fixture, ITestOutputHelper test)
    {
        fixture.SetOutput(test);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task UserService_Create_Should_Do_As_Expected()
    {
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();

        var user = new User
        {
            Username = "test",
            NormalizedUsername = "TEST",
            FullName = "Test Data",
            Email = "test@test.com"
        };

        await userService.CreateAsync(user, new CancellationToken());

        var result = await userService.GetByIdAsync(user.UserId, new CancellationToken());
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(user.UserId);
    }

    [Fact]
    public async Task UserService_GetByUsername_Should_Do_As_Expected()
    {
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();

        var user = _fixture.DataTests.First();
        var username = user.Username;

        var result = await userService.GetByUsernameAsync(username, new CancellationToken());
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(user.UserId);
    }

    [Fact]
    public async Task UserService_IsUserExist_Should_Do_As_Expected()
    {
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();

        var user = _fixture.DataTests[4];
        var username = user.Username;

        var result = await userService.GetByUsernameAsync(username, new CancellationToken());
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(user.UserId);
    }
}