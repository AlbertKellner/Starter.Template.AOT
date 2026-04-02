using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberUseCaseTests
{
    private readonly FakeLogger<NumberStringGetByNumberUseCase> _logger = new();
    private readonly NumberStringGetByNumberUseCase _useCase;

    public NumberStringGetByNumberUseCaseTests()
    {
        _useCase = new NumberStringGetByNumberUseCase(_logger);
    }

    [Fact]
    public void Execute_DeveRetornarUm_QuandoNumeroFor1()
    {
        var result = _useCase.Execute(1);

        Assert.NotNull(result);
        Assert.Equal("Um", result.Value);
    }

    [Fact]
    public void Execute_DeveRetornarDois_QuandoNumeroFor2()
    {
        var result = _useCase.Execute(2);

        Assert.NotNull(result);
        Assert.Equal("Dois", result.Value);
    }

    [Fact]
    public void Execute_DeveRetornarNull_QuandoNumeroNaoMapeado()
    {
        var result = _useCase.Execute(3);

        Assert.Null(result);
    }

    [Fact]
    public void Execute_DeveRegistrarLogInformation_QuandoNumeroEncontrado()
    {
        _useCase.Execute(1);

        var logs = _logger.GetSnapshot();

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
        _useCase.Execute(99);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("não possui mapeamento"));
    }
}
