using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberTextGet;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberTextGet;

public class NumberTextGetUseCaseTests
{
    private readonly FakeLogger<NumberTextGetUseCase> _logger = new();
    private readonly NumberTextGetUseCase _useCase;

    public NumberTextGetUseCaseTests()
    {
        _useCase = new NumberTextGetUseCase(_logger);
    }

    [Theory]
    [InlineData(1, "Um")]
    [InlineData(2, "Dois")]
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
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(-1)]
    [InlineData(100)]
    public void Execute_InvalidNumber_ThrowsArgumentOutOfRangeException(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _useCase.Execute(number));
    }
}
