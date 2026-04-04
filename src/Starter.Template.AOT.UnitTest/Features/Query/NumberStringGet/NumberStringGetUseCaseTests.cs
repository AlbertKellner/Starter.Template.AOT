using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCaseTests
{
    private static NumberStringGetUseCase CreateUseCase(FakeLogger<NumberStringGetUseCase> logger)
        => new(logger);

    [Fact]
    public void Execute_ComValor1_DeveRetornarUm()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        var output = useCase.Execute(1);

        Assert.NotNull(output);
        Assert.Equal("Um", output.Value);
    }

    [Fact]
    public void Execute_ComValor2_DeveRetornarDois()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        var output = useCase.Execute(2);

        Assert.NotNull(output);
        Assert.Equal("Dois", output.Value);
    }

    [Fact]
    public void Execute_ComValorNaoMapeado_DeveRetornarNulo()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        var output = useCase.Execute(99);

        Assert.Null(output);
    }

    [Fact]
    public void Execute_ComValorValido_DeveRegistrarLogInformationNoInicio()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Converter"));
    }

    [Fact]
    public void Execute_ComValorValido_DeveRegistrarLogInformationNoRetorno()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar string mapeada"));
    }

    [Fact]
    public void Execute_ComValorNaoMapeado_DeveRegistrarLogWarning()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(99);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("Retornar nulo"));
    }

    [Fact]
    public void Execute_DeveRegistrarLogsComPrefixoCorreto()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.All(logs, l => Assert.Contains("NumberStringGetUseCase", l.Message));
    }
}
