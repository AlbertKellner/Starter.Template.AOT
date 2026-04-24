using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DiskStructureGetByDrive;
using Starter.Template.AOT.Api.Shared.DiskAnalysis;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskStructureGetByDrive;

public class DiskStructureGetByDriveEndpointTests
{
    private readonly FakeLogger<DiskStructureGetByDriveEndpoint> _logger = new();

    [Fact]
    public async Task GetByDrive_WithChildren_ReturnsOkResult()
    {
        var structure = new DiskItemEntity
        {
            Name = "root",
            Children = [new DiskItemEntity { Name = "folder1", Size = 100 }]
        };
        var useCase = new StubDiskStructureGetByDriveUseCase(new DiskStructureGetByDriveOutput(structure));
        var endpoint = new DiskStructureGetByDriveEndpoint(useCase, _logger);

        var result = await endpoint.GetByDrive("C");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var entity = Assert.IsType<DiskItemEntity>(okResult.Value);
        Assert.Equal("root", entity.Name);
    }

    [Fact]
    public async Task GetByDrive_EmptyChildren_ReturnsNoContent()
    {
        var structure = new DiskItemEntity { Name = "root", Children = [] };
        var useCase = new StubDiskStructureGetByDriveUseCase(new DiskStructureGetByDriveOutput(structure));
        var endpoint = new DiskStructureGetByDriveEndpoint(useCase, _logger);

        var result = await endpoint.GetByDrive("Z");

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetByDrive_NullChildren_ReturnsNoContent()
    {
        var structure = new DiskItemEntity { Name = "root", Children = null };
        var useCase = new StubDiskStructureGetByDriveUseCase(new DiskStructureGetByDriveOutput(structure));
        var endpoint = new DiskStructureGetByDriveEndpoint(useCase, _logger);

        var result = await endpoint.GetByDrive("Z");

        Assert.IsType<NoContentResult>(result);
    }

    private class StubDiskStructureGetByDriveUseCase(DiskStructureGetByDriveOutput output) : IDiskStructureGetByDriveUseCase
    {
        public Task<DiskStructureGetByDriveOutput> ExecuteAsync(string selectedDrive) => Task.FromResult(output);
    }
}
