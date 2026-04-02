using Starter.Template.AOT.Api.Features.Query.NumberStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCaseTests
{
    private static NumberStringGetUseCase CreateUseCase(FakeLogger<NumberStringGetUseCase> logger)
    {
        return new NumberStringGetUseCase(logger);
    }

    [Fact]
    public void Execute_DeveRetornarUmQuandoValorForUm()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        var result = useCase.Execute(1);

        Assert.Equal(1, result.Value);
        Assert.Equal("Um", result.Text);
    }

    [Fact]
    public void Execute_DeveRetornarDoisQuandoValorForDois()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        var result = useCase.Execute(2);

        Assert.Equal(2, result.Value);
        Assert.Equal("Dois", result.Text);
    }

    [Fact]
    public void Execute_DeveLancarExcecaoQuandoValorNaoMapeado()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        Assert.Throws<KeyNotFoundException>(() => useCase.Execute(99));
    }

    [Fact]
    public void Execute_DeveRegistrarLogInformationNoInicio()
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
    public void Execute_DeveRegistrarLogInformationNoRetorno()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }

    [Fact]
    public void Execute_DeveRegistrarLogWarningQuandoValorNaoMapeado()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        try { useCase.Execute(99); } catch { }

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("não mapeado"));
    }
}
