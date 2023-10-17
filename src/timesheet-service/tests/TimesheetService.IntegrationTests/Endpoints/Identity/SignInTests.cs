using TimesheetService.Core.Abstractions;
using TimesheetService.Shared.Abstractions.Cache;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Contracts.Responses;
using TimesheetService.WebApi.Endpoints.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Endpoints.Identity;

public class SignInTests : IClassFixture<SignInFixture>
{
    private readonly SignInFixture _fixture;

    public SignInTests(SignInFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    /// <summary>
    /// Given :
    /// Invalid request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_InvalidRequest_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = string.Empty,
            Password = string.Empty
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithInvalidUsernameOrPassword_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "lorep",
            Password = "doloripsum"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return Ok.
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithCorrectUsernameAndPassword_ShouldReturn_LoginDto()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "admin",
            Password = "Qwerty@1234"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(OkObjectResult));
        var actual = (result.Result! as OkObjectResult)!;
        actual.Value.ShouldNotBeNull();
        actual.Value.ShouldBeOfType<LoginResponse>();

        var dto = (actual.Value! as LoginResponse)!;
        dto.UserId.ShouldBe(default);
        dto.AccessToken.ShouldNotBeNullOrWhiteSpace();
        dto.RefreshToken.ShouldNotBeNullOrWhiteSpace();
        dto.Expiry.ShouldBeGreaterThan(0);
    }
}