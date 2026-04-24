using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskStructureGetByDrive;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskStructureGetByDrive;

public class DiskStructureGetByDriveUseCaseTests
{
    private readonly FakeLogger<DiskStructureGetByDriveUseCase> _logger = new();

    [Fact]
    public async Task ExecuteAsync_ValidDrive_ReturnsStructureWithColors()
    {
        // Arrange
        var scanner = new FakeDiskScannerService(["C:\\"]);
        var useCase = new DiskStructureGetByDriveUseCase(scanner, _logger);

        // Act
        var result = await useCase.ExecuteAsync("C");

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Structure);
        Assert.Equal("root", result.Structure.Name);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Escanear drive"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Aplicar cores"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar estrutura do drive"));
    }
}
