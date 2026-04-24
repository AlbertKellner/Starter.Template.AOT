using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DiskStructureGetByFolder;
using Starter.Template.AOT.Api.Shared.DiskAnalysis;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskStructureGetByFolder;

public class DiskStructureGetByFolderEndpointTests
{
    private readonly FakeLogger<DiskStructureGetByFolderEndpoint> _logger = new();

    [Fact]
    public async Task GetByFolder_FolderFound_ReturnsOkResult()
    {
        var structure = new DiskItemEntity { Name = "target", Size = 500 };
        var useCase = new StubDiskStructureGetByFolderUseCase(new DiskStructureGetByFolderOutput(structure));
        var endpoint = new DiskStructureGetByFolderEndpoint(useCase, _logger);

        var result = await endpoint.GetByFolder("C", "target");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var entity = Assert.IsType<DiskItemEntity>(okResult.Value);
        Assert.Equal("target", entity.Name);
    }

    [Fact]
    public async Task GetByFolder_FolderNotFound_ReturnsNoContent()
    {
        var useCase = new StubDiskStructureGetByFolderUseCase(null);
        var endpoint = new DiskStructureGetByFolderEndpoint(useCase, _logger);

        var result = await endpoint.GetByFolder("C", "missing");

        Assert.IsType<NoContentResult>(result);
    }

    private class StubDiskStructureGetByFolderUseCase(DiskStructureGetByFolderOutput? output) : IDiskStructureGetByFolderUseCase
    {
        public Task<DiskStructureGetByFolderOutput?> ExecuteAsync(string selectedDrive, string selectedFolder) => Task.FromResult(output);
    }
}
