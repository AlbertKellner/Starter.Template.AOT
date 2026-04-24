using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskDrivesGetAll;

public class DiskDrivesGetAllEndpointTests
{
    private readonly FakeLogger<DiskDrivesGetAllEndpoint> _logger = new();

    [Fact]
    public void GetAll_WithDrives_ReturnsOkResult()
    {
        var useCase = new StubDiskDrivesGetAllUseCase(new DiskDrivesGetAllOutput(["C:\\", "D:\\"]));
        var endpoint = new DiskDrivesGetAllEndpoint(useCase, _logger);

        var result = endpoint.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var output = Assert.IsType<DiskDrivesGetAllOutput>(okResult.Value);
        Assert.Equal(2, output.Drives.Count);
    }

    [Fact]
    public void GetAll_NoDrives_ReturnsNoContent()
    {
        var useCase = new StubDiskDrivesGetAllUseCase(new DiskDrivesGetAllOutput([]));
        var endpoint = new DiskDrivesGetAllEndpoint(useCase, _logger);

        var result = endpoint.GetAll();

        Assert.IsType<NoContentResult>(result);
    }

    private class StubDiskDrivesGetAllUseCase(DiskDrivesGetAllOutput output) : IDiskDrivesGetAllUseCase
    {
        public DiskDrivesGetAllOutput Execute() => output;
    }
}
