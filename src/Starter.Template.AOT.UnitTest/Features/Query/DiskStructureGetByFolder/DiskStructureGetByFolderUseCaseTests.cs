using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskStructureGetByFolder;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskStructureGetByFolder;

public class DiskStructureGetByFolderUseCaseTests
{
    private readonly FakeLogger<DiskStructureGetByFolderUseCase> _logger = new();

    [Fact]
    public async Task ExecuteAsync_FolderExists_ReturnsStructure()
    {
        // Arrange
        var scanner = new FakeDiskScannerService(["C:\\"]);
        var useCase = new DiskStructureGetByFolderUseCase(scanner, _logger);

        // Act
        var result = await useCase.ExecuteAsync("C", "TestFolder");

        // Assert — FakeDiskScannerService.FindFolder returns the structure itself
        Assert.NotNull(result);
        Assert.NotNull(result.Structure);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Escanear drive"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Buscar pasta"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar estrutura da pasta"));
    }

    [Fact]
    public async Task ExecuteAsync_FolderNotFound_ReturnsNull()
    {
        // Arrange
        var scanner = new FakeDiskScannerService([], returnNullOnFindFolder: true);
        var useCase = new DiskStructureGetByFolderUseCase(scanner, _logger);

        // Act
        var result = await useCase.ExecuteAsync("C", "NonExistentFolder");

        // Assert
        Assert.Null(result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("não encontrada"));
    }
}
