using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberToStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberToStringGet;

public sealed class NumberToStringGetUseCaseTests
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

        Assert.NotNull(result);
        Assert.Equal("Um", result.Value);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Fact]
    public void Execute_Number2_ReturnsDois()
    {
        var result = _useCase.Execute(2);

        Assert.NotNull(result);
        Assert.Equal("Dois", result.Value);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Fact]
    public void Execute_UnmappedNumber_ReturnsNull()
    {
        var result = _useCase.Execute(99);

        Assert.Null(result);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("número não mapeado"));
    }
}
