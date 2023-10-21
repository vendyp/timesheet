using Bogus;
using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Databases;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Services;

[CollectionDefinition(nameof(UserServiceFixture))]
public class UserServiceFixture : BaseServiceFixture, ICollectionFixture<UserServiceFixture>
{
    public UserServiceFixture() : base()
    {
        ContextId = Guid.NewGuid();
        DataTests = new List<User>();
    }


    public override void SetOutput(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public Guid ContextId { get; }

    public List<User> DataTests { get; }

    private readonly Faker<User> _userGenerator =
        new Faker<User>()
            .RuleFor(e => e.Username, f => f.Person.UserName)
            .RuleFor(e => e.NormalizedUsername, (_, u) => u.Username.ToUpper())
            .RuleFor(e => e.Password, f => f.Internet.Password())
            .RuleFor(e => e.FullName, f => f.Person.FullName)
            .RuleFor(e => e.Email, f => f.Person.Email)
            .UseSeed(50);

    public override void ConstructFixture()
    {
        if (OutputHelper != null)
        {
            Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(OutputHelper));
        }

        ServiceProvider = Services.BuildServiceProvider();

        var dbContext = ServiceProvider.GetRequiredService<IDbContext>();

        var users = _userGenerator.GenerateBetween(1, 20);
        foreach (var item in users)
        {
            DataTests.Add(item);
            dbContext.Insert(item);
        }

        dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public override ServiceProvider ServiceProvider { get; set; } = null!;
    public override ITestOutputHelper? OutputHelper { get; set; }
}