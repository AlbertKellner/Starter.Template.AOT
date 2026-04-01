using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCaseTests
{
    private static NumberStringGetUseCase CreateUseCase(FakeLogger<NumberStringGetUseCase>? logger = null)
    {
        return new NumberStringGetUseCase(logger ?? new FakeLogger<NumberStringGetUseCase>());
    }

    [Fact]
    public void Execute_DeveRetornarUm_QuandoNumeroFor1()
    {
        var useCase = CreateUseCase();

        var result = useCase.Execute(1);

        Assert.NotNull(result);
        Assert.Equal("Um", result.Value);
    }

    [Fact]
    public void Execute_DeveRetornarDois_QuandoNumeroFor2()
    {
        var useCase = CreateUseCase();

        var result = useCase.Execute(2);

        Assert.NotNull(result);
        Assert.Equal("Dois", result.Value);
    }

    [Fact]
    public void Execute_DeveRetornarNull_QuandoNumeroNaoMapeado()
    {
        var useCase = CreateUseCase();

        var result = useCase.Execute(3);

        Assert.Null(result);
    }

    [Fact]
    public void Execute_DeveRegistrarLogInformation_QuandoNumeroMapeado()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(1);

        var logs = fakeLogger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Converter"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar valor"));
    }

    [Fact]
    public void Execute_DeveRegistrarLogWarning_QuandoNumeroNaoMapeado()
    {
        var fakeLogger = new FakeLogger<NumberStringGetUseCase>();
        var useCase = CreateUseCase(fakeLogger);

        useCase.Execute(99);

        var logs = fakeLogger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("não possui mapeamento"));
    }
}
