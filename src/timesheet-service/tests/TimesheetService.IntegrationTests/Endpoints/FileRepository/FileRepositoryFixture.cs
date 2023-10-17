using Meziantou.Extensions.Logging.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Endpoints.FileRepository;

[CollectionDefinition(nameof(FileRepositoryFixture))]
public class FileRepositoryFixture : BaseServiceFixture, ICollectionFixture<FileRepositoryFixture>
{
    public FileRepositoryFixture() : base(nameof(FileRepositoryFixture))
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