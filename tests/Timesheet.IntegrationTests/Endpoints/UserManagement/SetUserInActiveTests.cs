using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.Shared.Abstractions.Enums;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Timesheet.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class SetUserInActiveTests
{
    private readonly UserManagementFixture _fixture;

    public SetUserInActiveTests(UserManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task SetUserInActive_Should_Do_Correctly()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var request = new SetUserInActiveRequest
        {
            UserId = Guid.Empty
        };

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        //first time get
        var user = await dbContext.Set<User>().Where(e => e.UserId == request.UserId)
            .FirstOrDefaultAsync(CancellationToken.None);
        user.ShouldNotBeNull();
        user.StatusRecord.ShouldBe(StatusRecord.Active);

        var setUserInActive = new SetUserInActive(dbContext);

        var result = await setUserInActive.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        //second time get
        user = await dbContext.Set<User>().Where(e => e.UserId == request.UserId)
            .FirstOrDefaultAsync(CancellationToken.None);
        user.ShouldNotBeNull();
        user.StatusRecord.ShouldBe(StatusRecord.InActive);
    }
}