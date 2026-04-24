using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskDrivesGetAll;

public class DiskDrivesGetAllUseCaseTests
{
    private readonly FakeLogger<DiskDrivesGetAllUseCase> _logger = new();

    [Fact]
    public void Execute_WithDrivesAvailable_ReturnsDriveList()
    {
        // Arrange
        var scanner = new FakeDiskScannerService(["C:\\", "D:\\"]);
        var useCase = new DiskDrivesGetAllUseCase(scanner, _logger);

        // Act
        var result = useCase.Execute();

        // Assert
        Assert.Equal(2, result.Drives.Count);
        Assert.Contains("C:\\", result.Drives);
        Assert.Contains("D:\\", result.Drives);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Obter lista de drives"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar 2 drives"));
    }

    [Fact]
    public void Execute_WithNoDrives_ReturnsEmptyList()
    {
        // Arrange
        var scanner = new FakeDiskScannerService([]);
        var useCase = new DiskDrivesGetAllUseCase(scanner, _logger);

        // Act
        var result = useCase.Execute();

        // Assert
        Assert.Empty(result.Drives);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar 0 drives"));
    }
}
