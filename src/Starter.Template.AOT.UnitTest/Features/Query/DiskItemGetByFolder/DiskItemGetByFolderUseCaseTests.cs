using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;
using Starter.Template.AOT.Api.Shared.DiskItem;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskItemGetByFolder;

public sealed class DiskItemGetByFolderUseCaseTests
{
    private sealed class FakeRepository(DiskItemEntity? entity) : IDiskItemGetByFolderRepository
    {
        public Task<DiskItemEntity?> GetAsync(int driveIndex, string folderPath) =>
            Task.FromResult(entity);
    }

    private static DiskItemEntity BuildFakeFolder()
    {
        var folder = new DiskItemEntity { Name = "myfolder", FullPath = "/myfolder", Size = 500, Children = [] };
        folder.Children.Add(new DiskItemEntity { Name = "file1.txt", Size = 200, Extension = ".txt", FullPath = "/myfolder/file1.txt", Children = [] });
        folder.Children.Add(new DiskItemEntity { Name = "file2.txt", Size = 300, Extension = ".txt", FullPath = "/myfolder/file2.txt", Children = [] });
        return folder;
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingFolder_ReturnsOutput()
    {
        var logger = new FakeLogger<DiskItemGetByFolderUseCase>();
        var repository = new FakeRepository(BuildFakeFolder());
        var useCase = new DiskItemGetByFolderUseCase(repository, logger);

        var result = await useCase.ExecuteAsync(0, "myfolder");

        Assert.NotNull(result);
        Assert.Equal("myfolder", result.Name);
    }

    [Fact]
    public async Task ExecuteAsync_WithMissingFolder_ReturnsNull()
    {
        var logger = new FakeLogger<DiskItemGetByFolderUseCase>();
        var repository = new FakeRepository(null);
        var useCase = new DiskItemGetByFolderUseCase(repository, logger);

        var result = await useCase.ExecuteAsync(0, "nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingFolder_MapsChildren()
    {
        var logger = new FakeLogger<DiskItemGetByFolderUseCase>();
        var repository = new FakeRepository(BuildFakeFolder());
        var useCase = new DiskItemGetByFolderUseCase(repository, logger);

        var result = await useCase.ExecuteAsync(0, "myfolder");

        Assert.NotNull(result!.Children);
        Assert.Equal(2, result.Children!.Count);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCalled_LogsInformationAtSearch()
    {
        var logger = new FakeLogger<DiskItemGetByFolderUseCase>();
        var repository = new FakeRepository(BuildFakeFolder());
        var useCase = new DiskItemGetByFolderUseCase(repository, logger);

        await useCase.ExecuteAsync(0, "myfolder");

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Buscar"));
    }
}
