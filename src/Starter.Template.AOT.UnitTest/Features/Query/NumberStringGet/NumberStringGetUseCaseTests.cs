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

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(1, "Um")]
    [InlineData(2, "Dois")]
    [InlineData(3, "Três")]
    [InlineData(4, "Quatro")]
    [InlineData(5, "Cinco")]
    [InlineData(6, "Seis")]
    [InlineData(7, "Sete")]
    [InlineData(8, "Oito")]
    [InlineData(9, "Nove")]
    [InlineData(10, "Dez")]
    public void Execute_ValidNumber_ReturnsExpectedString(int number, string expected)
    {
        var result = _useCase.Execute(number);

        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Execute_ValidNumber_LogsProcessingAndResult()
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

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(100)]
    public void Execute_InvalidNumber_ThrowsArgumentOutOfRangeException(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _useCase.Execute(number));
    }
}
