using Bogus;
using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TimesheetService.IntegrationTests.Services;

[Collection(nameof(GeneralServiceFixture))]
public class FileRepositoryServiceTests
{
    private readonly GeneralServiceFixture _fixture;

    public FileRepositoryServiceTests(GeneralServiceFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    private readonly Faker<FileRepository> _fileRepositoryGenerator =
        new Faker<FileRepository>()
            .RuleFor(e => e.FileName, faker => faker.System.FileName())
            .RuleFor(e => e.UniqueFileName,
                (_, repository) => $"{Guid.NewGuid()}{Path.GetExtension(repository.FileName)}")
            .RuleFor(e => e.FileExtension, (_, repository) => Path.GetExtension(repository.FileName))
            .RuleFor(e => e.Size, faker => faker.Random.Number(1024, int.MaxValue))
            .UseSeed(50);

    [Fact]
    public async Task FileRepositoryService_TotalUsedStorage_Should_Do_As_Expected()
    {
        var fileRepositoryService = _fixture.ServiceProvider.GetRequiredService<IFileRepositoryService>();

        long size = 0;
        for (var i = 0; i < 50; i++)
        {
            var fileRepository = _fileRepositoryGenerator.Generate();
            size += fileRepository.Size;
            await fileRepositoryService.CreateAsync(fileRepository, CancellationToken.None);
        }

        var result = await fileRepositoryService.TotalUsedStorageAsync(CancellationToken.None);
        result.ShouldBe(size);
    }
}