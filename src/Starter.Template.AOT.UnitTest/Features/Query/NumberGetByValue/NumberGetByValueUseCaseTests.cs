using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberGetByValue;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberGetByValue;

public class NumberGetByValueUseCaseTests
{
    private readonly FakeLogger<NumberGetByValueUseCase> _logger = new();
    private readonly NumberGetByValueUseCase _useCase;

    public NumberGetByValueUseCaseTests()
    {
        _useCase = new NumberGetByValueUseCase(_logger);
    }

    [Fact]
    public void Execute_Number1_ReturnsUm()
    {
        var result = _useCase.Execute(1);

        Assert.Equal("Um", result.Value);
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

    [Fact]
    public void Execute_Number1_LogsProcessingAndResult()
    {
        _useCase.Execute(1);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Processar conversão"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar valor convertido"));
    }
}
