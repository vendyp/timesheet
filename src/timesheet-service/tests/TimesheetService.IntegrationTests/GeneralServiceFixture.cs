using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests;

[CollectionDefinition(nameof(GeneralServiceFixture))]
public class GeneralServiceFixture : BaseServiceFixture, ICollectionFixture<GeneralServiceFixture>
{
    public GeneralServiceFixture() : base(nameof(GeneralServiceFixture))
    {
    }

    public override void ConstructFixture()
    {
        if (OutputHelper != null)
        {
            Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(OutputHelper));
        }

        ServiceProvider = Services.BuildServiceProvider();
    }

    public override void SetOutput(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override ServiceProvider ServiceProvider { get; set; } = null!;
    public override ITestOutputHelper? OutputHelper { get; set; }
}