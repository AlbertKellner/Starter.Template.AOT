using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.DiskDrivesGetAll;

public sealed class DiskDrivesGetAllUseCaseTests
{
    [Fact]
    public void Execute_DeveRetornarListaDeDrives()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var useCase = new DiskDrivesGetAllUseCase(logger);

        var result = useCase.Execute();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Execute_DeveAtribuirIndicesSequenciais()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var useCase = new DiskDrivesGetAllUseCase(logger);

        var result = useCase.Execute();

        for (var i = 0; i < result.Count; i++)
            Assert.Equal(i, result[i].Index);
    }

    [Fact]
    public void Execute_DeveRegistrarLogInformationNoInicio()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var useCase = new DiskDrivesGetAllUseCase(logger);

        useCase.Execute();

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Listar"));
    }

    [Fact]
    public void Execute_DeveRegistrarLogInformationNoRetorno()
    {
        var logger = new FakeLogger<DiskDrivesGetAllUseCase>();
        var useCase = new DiskDrivesGetAllUseCase(logger);

        useCase.Execute();

        var logs = logger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }
}
