using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Timesheet.IntegrationTests.Endpoints.RoleManagement;

[CollectionDefinition(nameof(RoleManagementFixture))]
public class RoleManagementFixture : BaseServiceFixture, ICollectionFixture<RoleManagementFixture>
{
    public RoleManagementFixture() : base(nameof(RoleManagementFixture))
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