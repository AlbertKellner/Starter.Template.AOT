using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;
using Starter.Template.AOT.Api.Shared.DiskItem;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskItemsGetAll;

public sealed class DiskItemsGetAllUseCaseTests
{
    private sealed class FakeRepository(DiskItemEntity entity) : IDiskItemsGetAllRepository
    {
        public Task<DiskItemEntity> GetByDriveIndexAsync(int driveIndex) =>
            Task.FromResult(entity);
    }

    private static DiskItemEntity BuildFakeRoot()
    {
        var root = new DiskItemEntity { Name = "root", FullPath = "/", Children = [] };
        root.Children.Add(new DiskItemEntity { Name = "folder1", FullPath = "/folder1", Size = 1024, Children = [] });
        root.Children.Add(new DiskItemEntity { Name = "folder2", FullPath = "/folder2", Size = 2048, Children = [] });
        return root;
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarOutputComNomeRoot()
    {
        var logger = new FakeLogger<DiskItemsGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeRoot());
        var useCase = new DiskItemsGetAllUseCase(repository, logger);

        var result = await useCase.ExecuteAsync(0);

        Assert.NotNull(result);
        Assert.Equal("root", result.Name);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAtribuirCoresAosFilhosRaiz()
    {
        var logger = new FakeLogger<DiskItemsGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeRoot());
        var useCase = new DiskItemsGetAllUseCase(repository, logger);

        var result = await useCase.ExecuteAsync(0);

        Assert.NotNull(result.Children);
        Assert.All(result.Children!, child => Assert.False(string.IsNullOrEmpty(child.Color)));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRegistrarLogInformationNoInicio()
    {
        var logger = new FakeLogger<DiskItemsGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeRoot());
        var useCase = new DiskItemsGetAllUseCase(repository, logger);

        await useCase.ExecuteAsync(0);

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Obter"));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRegistrarLogInformationNoRetorno()
    {
        var logger = new FakeLogger<DiskItemsGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeRoot());
        var useCase = new DiskItemsGetAllUseCase(repository, logger);

        await useCase.ExecuteAsync(0);

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }
}
