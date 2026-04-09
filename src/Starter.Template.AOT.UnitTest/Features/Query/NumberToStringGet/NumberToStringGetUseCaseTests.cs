using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberToStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberToStringGet;

public class NumberToStringGetUseCaseTests
{
    private readonly FakeLogger<NumberToStringGetUseCase> _logger = new();
    private readonly NumberToStringGetUseCase _useCase;

    public NumberToStringGetUseCaseTests()
    {
        _useCase = new NumberToStringGetUseCase(_logger);
    }

    [Fact]
    public void Execute_Number1_ReturnsUm()
    {
        var result = _useCase.Execute(1);

        Assert.Equal("Um", result.Result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Processar conversão"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Fact]
    public void Execute_Number2_ReturnsDois()
    {
        var result = _useCase.Execute(2);

        Assert.Equal("Dois", result.Result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Processar conversão"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(-1)]
    public void Execute_UnsupportedNumber_ThrowsArgumentOutOfRangeException(int number)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _useCase.Execute(number));

        Assert.Contains("Número não suportado", exception.Message);
    }
}
