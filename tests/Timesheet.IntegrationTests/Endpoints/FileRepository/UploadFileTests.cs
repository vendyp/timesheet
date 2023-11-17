using Timesheet.Shared.Abstractions.Databases;
using Timesheet.Shared.Abstractions.Files;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Contracts.Responses;
using Timesheet.WebApi.Endpoints.FileRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Timesheet.IntegrationTests.Endpoints.FileRepository;

[Collection(nameof(FileRepositoryFixture))]
public class UploadFileTests
{
    private readonly FileRepositoryFixture _fixture;

    public UploadFileTests(FileRepositoryFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task UploadFile_ShouldBe_Correct()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        //Act
        var content = "Hello World from a Fake File";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        //create FormFile with desired data
        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var request = new UploadFileRequest
        {
            File = file,
            Source = "test"
        };

        var uploadFile = new UploadFile(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IFileService>());

        var result = await uploadFile.HandleAsync(request, CancellationToken.None);
        // Assert the expected results
        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeOfType<UploadFileResponse>();

        var data = await scope.ServiceProvider.GetRequiredService<IDbContext>().Set<Domain.Entities.FileRepository>()
            .Where(e => e.FileRepositoryId == result.Value.FileId).FirstOrDefaultAsync(CancellationToken.None);

        data.ShouldNotBeNull();
    }
}