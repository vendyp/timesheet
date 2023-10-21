using Bogus;
using Microsoft.Extensions.DependencyInjection;
using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Services;

[Collection(nameof(GeneralServiceFixture))]
public class TimesheetServiceTests
{
    private readonly GeneralServiceFixture _fixture;
    private const int TotalData = 20;

    public TimesheetServiceTests(GeneralServiceFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task Create_Should_Do_As_Expected()
    {
        var service = _fixture.ServiceProvider.GetRequiredService<ITimesheetService>();

        var timesheetGenerator = new Faker<Timesheet>()
            .RuleFor(e => e.UserId, _ => Guid.Empty)
            .RuleFor(e => e.Title, faker => faker.Lorem.Word())
            .RuleFor(e => e.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(e => e.TotalTime, faker => faker.Random.Decimal(0m, 8m))
            .UseSeed(TotalData);

        for (var i = 0; i < TotalData; i++)
            await service.CreateAsync(timesheetGenerator.Generate(), CancellationToken.None);
    }

    [Fact]
    public async Task GetTotalHoursByUserId_Should_Do_As_Expected()
    {
        var userGenerator =
            new Faker<User>()
                .RuleFor(e => e.Username, f => f.Person.UserName)
                .RuleFor(e => e.NormalizedUsername, (_, u) => u.Username.ToUpper())
                .RuleFor(e => e.Password, f => f.Internet.Password())
                .RuleFor(e => e.FullName, f => f.Person.FullName)
                .RuleFor(e => e.Email, f => f.Person.Email)
                .UseSeed(5);
        var timesheetService = _fixture.ServiceProvider.GetRequiredService<ITimesheetService>();
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();
        var user = userGenerator.Generate();
        await userService.CreateAsync(user, CancellationToken.None);

        var timesheetGenerator = new Faker<Timesheet>()
            .RuleFor(e => e.UserId, _ => user.UserId)
            .RuleFor(e => e.Title, faker => faker.Lorem.Word())
            .RuleFor(e => e.Description, faker => faker.Lorem.Paragraph())
            .RuleFor(e => e.TotalTime, faker => Math.Round(faker.Random.Decimal(0m, 8m), 2))
            .UseSeed(TotalData);

        decimal totalHours = 0m;
        for (var i = 0; i < TotalData; i++)
        {
            var timesheet = timesheetGenerator.Generate();
            await timesheetService.CreateAsync(timesheet, CancellationToken.None);
            totalHours += timesheet.TotalTime;
        }

        var result = await timesheetService.GetTotalHoursByUserIdAsync(user.UserId, CancellationToken.None);

        //because entity is rounded with precision 18,2
        _fixture.OutputHelper!.WriteLine($"Total expected totalHours before rounded {totalHours}");
        var rounded = Math.Round(totalHours, 2);
        result.ShouldBe(rounded);
    }
}