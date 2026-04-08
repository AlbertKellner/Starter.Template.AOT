using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberToStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberToStringGet;

public sealed class NumberToStringGetUseCaseTests
{
    private readonly FakeLogger<NumberToStringGetUseCase> _logger = new();
    private readonly NumberToStringGetUseCase _sut;

    public NumberToStringGetUseCaseTests()
    {
        _sut = new NumberToStringGetUseCase(_logger);
    }

    [Fact]
    public void Execute_Number1_ReturnsUm()
    {
        var result = _sut.Execute(1);

        Assert.Equal("Um", result);
        Assert.Contains(_logger.GetSnapshot(), l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Converter número para string"));
    }

    [Fact]
    public void Execute_Number2_ReturnsDois()
    {
        var result = _sut.Execute(2);

        Assert.Equal("Dois", result);
        Assert.Contains(_logger.GetSnapshot(), l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Fact]
    public void Execute_UnsupportedNumber_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _sut.Execute(99));
    }
}
