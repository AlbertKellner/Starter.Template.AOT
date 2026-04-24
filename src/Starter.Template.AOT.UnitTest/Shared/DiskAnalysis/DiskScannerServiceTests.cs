using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Shared.DiskAnalysis;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Shared.DiskAnalysis;

public class DiskScannerServiceTests : IDisposable
{
    private readonly FakeLogger<DiskScannerService> _logger = new();
    private readonly DiskScannerService _service;
    private readonly string _testRoot;

    public DiskScannerServiceTests()
    {
        _service = new DiskScannerService(_logger);
        _testRoot = Path.Combine(Path.GetTempPath(), $"DiskScannerTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testRoot);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testRoot))
            Directory.Delete(_testRoot, recursive: true);
    }

    private void CreateTestFile(string relativePath, int sizeBytes)
    {
        var fullPath = Path.Combine(_testRoot, relativePath);
        var dir = Path.GetDirectoryName(fullPath)!;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllBytes(fullPath, new byte[sizeBytes]);
    }

    private void CreateTestDirectory(string relativePath)
    {
        var fullPath = Path.Combine(_testRoot, relativePath);
        Directory.CreateDirectory(fullPath);
    }

    [Fact]
    public void GetDriveNames_ReturnsAtLeastOneDrive()
    {
        var result = _service.GetDriveNames();

        Assert.NotEmpty(result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Listar drives"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("drives encontrados"));
    }

    [Fact]
    public async Task ScanDriveAsync_WithFilesAndFolders_ReturnsCorrectStructure()
    {
        // Arrange — create real files and folders on disk
        CreateTestFile("docs/readme.txt", 100);
        CreateTestFile("docs/notes.txt", 200);
        CreateTestFile("images/photo.jpg", 5000);
        CreateTestFile("images/icons/logo.png", 1500);
        CreateTestFile("report.pdf", 3000);
        CreateTestDirectory("empty-folder");

        // Act
        var result = await _service.ScanDriveAsync(_testRoot);

        // Assert
        Assert.Equal("root", result.Name);
        Assert.Equal(_testRoot, result.FullPath);
        Assert.NotNull(result.Children);
        Assert.True(result.Children.Count > 0);

        // Verify total size is sum of all files
        Assert.Equal(100 + 200 + 5000 + 1500 + 3000, result.Size);

        // Verify children are sorted by size descending
        for (var i = 0; i < result.Children.Count - 1; i++)
        {
            Assert.True(result.Children[i].Size >= result.Children[i + 1].Size);
        }

        // Verify docs folder
        var docsFolder = result.Children.FirstOrDefault(c => c.Name == "docs");
        Assert.NotNull(docsFolder);
        Assert.True(docsFolder.IsFolder);
        Assert.Equal(300, docsFolder.Size);
        Assert.Equal(2, docsFolder.Children!.Count);

        // Verify images folder with nested subfolder
        var imagesFolder = result.Children.FirstOrDefault(c => c.Name == "images");
        Assert.NotNull(imagesFolder);
        Assert.Equal(6500, imagesFolder.Size);

        var iconsFolder = imagesFolder.Children!.FirstOrDefault(c => c.Name == "icons");
        Assert.NotNull(iconsFolder);
        Assert.Equal(1500, iconsFolder.Size);

        // Verify file at root level
        var reportFile = result.Children.FirstOrDefault(c => c.Name == "report.pdf");
        Assert.NotNull(reportFile);
        Assert.Equal(3000, reportFile.Size);
        Assert.Equal(".pdf", reportFile.Extension);
        Assert.False(reportFile.IsFolder);

        // Verify empty folder exists
        var emptyFolder = result.Children.FirstOrDefault(c => c.Name == "empty-folder");
        Assert.NotNull(emptyFolder);
        Assert.True(emptyFolder.IsFolder);
        Assert.Equal(0, emptyFolder.Size);

        // Verify logs
        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Iniciar escaneamento"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar estrutura escaneada"));
    }

    [Fact]
    public async Task ScanDriveAsync_NonExistentPath_ReturnsEmptyRoot()
    {
        var nonExistentPath = Path.Combine(_testRoot, "does-not-exist");

        var result = await _service.ScanDriveAsync(nonExistentPath);

        Assert.Equal("root", result.Name);
        Assert.NotNull(result.Children);
        Assert.Empty(result.Children);
    }

    [Fact]
    public async Task ScanDriveAsync_EmptyDirectory_ReturnsEmptyChildren()
    {
        var result = await _service.ScanDriveAsync(_testRoot);

        Assert.Equal("root", result.Name);
        Assert.NotNull(result.Children);
        Assert.Empty(result.Children);
    }

    [Fact]
    public async Task ScanDriveAsync_FilesOnly_ReturnsFilesWithCorrectExtensions()
    {
        CreateTestFile("data.csv", 400);
        CreateTestFile("config.json", 150);

        var result = await _service.ScanDriveAsync(_testRoot);

        Assert.Equal(2, result.Children!.Count);

        var csvFile = result.Children.First(c => c.Name == "data.csv");
        Assert.Equal(".csv", csvFile.Extension);
        Assert.Equal(400, csvFile.Size);

        var jsonFile = result.Children.First(c => c.Name == "config.json");
        Assert.Equal(".json", jsonFile.Extension);
        Assert.Equal(150, jsonFile.Size);
    }

    [Fact]
    public void FindFolder_ExistingFolder_ReturnsFolder()
    {
        var target = new DiskItemEntity { Name = "SubDir", Extension = "", Children = [] };
        var root = new DiskItemEntity
        {
            Name = "root",
            Children = [target]
        };

        var result = _service.FindFolder(root, "SubDir");

        Assert.NotNull(result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("encontrada"));
    }

    [Fact]
    public void FindFolder_NonExistentFolder_ReturnsNull()
    {
        var root = new DiskItemEntity { Name = "root", Children = [] };

        var result = _service.FindFolder(root, "Missing");

        Assert.Null(result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("não encontrada"));
    }

    [Fact]
    public async Task ScanDriveAsync_NestedFolders_CalculatesSizesCorrectly()
    {
        CreateTestFile("level1/level2/level3/deep.bin", 800);
        CreateTestFile("level1/level2/mid.bin", 400);
        CreateTestFile("level1/top.bin", 200);

        var result = await _service.ScanDriveAsync(_testRoot);

        var level1 = result.Children!.First(c => c.Name == "level1");
        Assert.Equal(1400, level1.Size);

        var level2 = level1.Children!.First(c => c.Name == "level2");
        Assert.Equal(1200, level2.Size);

        var level3 = level2.Children!.First(c => c.Name == "level3");
        Assert.Equal(800, level3.Size);
    }
}
