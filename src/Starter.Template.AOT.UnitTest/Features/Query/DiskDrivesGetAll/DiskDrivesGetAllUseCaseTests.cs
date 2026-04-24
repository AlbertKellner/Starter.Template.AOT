using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskDrivesGetAll;

public sealed class DiskDrivesGetAllUseCaseTests
{
    private sealed class FakeRepository(DriveInfo[] drives) : IDiskDrivesGetAllRepository
    {
        public DriveInfo[] GetAllDrives() => drives;
    }

    private static DriveInfo[] BuildFakeDrives() => [new DriveInfo("/")];

    [Fact]
    public void Execute_WithAvailableDrives_ReturnsDriveList()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeDrives());
        var useCase = new DiskDrivesGetAllUseCase(repository, logger);

        var result = useCase.Execute();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Execute_WithAvailableDrives_AssignsSequentialIndexes()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeDrives());
        var useCase = new DiskDrivesGetAllUseCase(repository, logger);

        var result = useCase.Execute();

        for (var i = 0; i < result.Count; i++)
            Assert.Equal(i, result[i].Index);
    }

    [Fact]
    public void Execute_WhenCalled_LogsInformationAtStart()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeDrives());
        var useCase = new DiskDrivesGetAllUseCase(repository, logger);

        useCase.Execute();

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Listar"));
    }

    [Fact]
    public void Execute_WhenCalled_LogsInformationAtReturn()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var repository = new FakeRepository(BuildFakeDrives());
        var useCase = new DiskDrivesGetAllUseCase(repository, logger);

        useCase.Execute();

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }
}
