using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGet;

public class NumberStringGetUseCaseTests
{
    private readonly FakeLogger<NumberStringGetUseCase> _logger = new();
    private readonly NumberStringGetUseCase _useCase;

    public NumberStringGetUseCaseTests()
    {
        _useCase = new NumberStringGetUseCase(_logger);
    }

    [Fact]
    public void Execute_Number1_ReturnsUm()
    {
        var result = _useCase.Execute(1);

        Assert.Equal("Um", result.Value);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Processar conversão"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar valor convertido"));
    }

    [Fact]
    public void Execute_Number2_ReturnsDois()
    {
        var result = _useCase.Execute(2);

        Assert.Equal("Dois", result.Value);
    }

    [Fact]
    public void Execute_InvalidNumber_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _useCase.Execute(3));
    }
}
