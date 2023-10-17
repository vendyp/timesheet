using TimesheetService.Core.Abstractions;
using TimesheetService.IntegrationTests.Helpers;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Endpoints.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Endpoints.Identity;

public class ChangePasswordTests : IClassFixture<ChangePasswordFixture>
{
    private readonly ChangePasswordFixture _fixture;

    public ChangePasswordTests(ChangePasswordFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            // all empty
            new ChangePasswordRequest
            {
                CurrentPassword = "",
                NewPassword = ""
            }
        };
        yield return new object[]
        {
            // current and new same value
            new ChangePasswordRequest
            {
                CurrentPassword = "Qwerty@12345",
                NewPassword = "Qwerty@12345"
            }
        };
        yield return new object[]
        {
            new ChangePasswordRequest
            {
                CurrentPassword = "Qwerty@12345",
                NewPassword = ""
            }
        };
        yield return new object[]
        {
            new ChangePasswordRequest
            {
                CurrentPassword = "",
                NewPassword = "Qwerty@12345"
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task ChangePassword_Given_InvalidRequest_ShouldReturn_BadRequest(ChangePasswordRequest request)
    {
        // Arrange
        var changePassword = new ChangePassword(
            _fixture.ServiceProvider.GetRequiredService<IUserService>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>(),
            _fixture.ServiceProvider.GetRequiredService<IClock>(),
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        // Act
        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// This may not be happening because Identity.Id will always validated through authorization
    /// </summary>
    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithInvalidInjector_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerytgsdgw", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            _fixture.ServiceProvider.GetRequiredService<IUserService>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>(),
            _fixture.ServiceProvider.GetRequiredService<IClock>(),
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.NewGuid()));

        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithInvalidPassword_ShouldReturn_BadRequest()
    {
        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerytgsdgw", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            _fixture.ServiceProvider.GetRequiredService<IUserService>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>(),
            _fixture.ServiceProvider.GetRequiredService<IClock>(),
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
        var obj = (actual.Value as Error);
        obj.ShouldNotBeNull();
        obj.Message.ShouldBe("Invalid password");
    }

    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithCorrectPassword_ShouldReturn_Ok()
    {
        // Arrange
        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerty@1234", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            _fixture.ServiceProvider.GetRequiredService<IUserService>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>(),
            _fixture.ServiceProvider.GetRequiredService<IClock>(),
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        var result = await changePassword.HandleAsync(request);

        if (result is BadRequestObjectResult errObj)
        {
            var err = errObj.Value as Error;
            _fixture.OutputHelper!.WriteLine(err!.Message);
        }

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(OkResult));
        var actual = (result as OkResult)!;
        actual.StatusCode.ShouldBe(200);
    }
}