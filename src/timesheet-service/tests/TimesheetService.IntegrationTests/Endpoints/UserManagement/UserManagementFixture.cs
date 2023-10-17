using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Endpoints.UserManagement;

[CollectionDefinition(nameof(UserManagementFixture))]
public class UserManagementFixture : BaseServiceFixture, ICollectionFixture<UserManagementFixture>
{
    public UserManagementFixture() : base(nameof(UserManagementFixture))
    {
    }

    public override void SetOutput(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override void ConstructFixture()
    {
        if (OutputHelper != null)
        {
            Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(OutputHelper));
        }

        ServiceProvider = Services.BuildServiceProvider();
    }

    public override ServiceProvider ServiceProvider { get; set; } = null!;
    public override ITestOutputHelper? OutputHelper { get; set; }
}