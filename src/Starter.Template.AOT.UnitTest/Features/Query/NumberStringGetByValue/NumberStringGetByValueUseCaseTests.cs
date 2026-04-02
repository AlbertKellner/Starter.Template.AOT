using Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;
using Starter.Template.AOT.UnitTest.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByValue;

public sealed class NumberStringGetByValueUseCaseTests
{
    private readonly FakeLogger<NumberStringGetByValueUseCase> _logger = new();
    private readonly NumberStringGetByValueUseCase _useCase;

    public NumberStringGetByValueUseCaseTests()
    {
        _useCase = new NumberStringGetByValueUseCase(_logger);
    }

    [Fact]
    public void Execute_ComValor1_DeveRetornarUm()
    {
        var result = _useCase.Execute(1);

        Assert.NotNull(result);
        Assert.Equal("Um", result.Value);
    }

    [Fact]
    public void Execute_ComValor2_DeveRetornarDois()
    {
        var result = _useCase.Execute(2);

        Assert.NotNull(result);
        Assert.Equal("Dois", result.Value);
    }

    [Fact]
    public void Execute_ComValorNaoMapeado_DeveRetornarNulo()
    {
        var result = _useCase.Execute(3);

        Assert.Null(result);
    }

    [Fact]
    public void Execute_ComValor1_DeveRegistrarLogInformation()
    {
        _useCase.Execute(1);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Buscar representação textual"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar representação encontrada"));
    }

    [Fact]
    public void Execute_ComValorNaoMapeado_DeveRegistrarLogWarning()
    {
        _useCase.Execute(99);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("número não mapeado"));
    }
}
