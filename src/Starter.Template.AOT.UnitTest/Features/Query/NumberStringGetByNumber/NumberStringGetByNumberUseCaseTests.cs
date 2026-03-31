using Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberUseCaseTests
{
    private readonly NumberStringGetByNumberUseCase _sut;
    private readonly FakeLogger<NumberStringGetByNumberUseCase> _logger;

    public NumberStringGetByNumberUseCaseTests()
    {
        _logger = new FakeLogger<NumberStringGetByNumberUseCase>();
        _sut = new NumberStringGetByNumberUseCase(_logger);
    }

    [Fact]
    public void Execute_ComNumero1_DeveRetornarUm()
    {
        var result = _sut.Execute(1);

        Assert.NotNull(result);
        Assert.Equal("Um", result.NumberAsString);
    }

    [Fact]
    public void Execute_ComNumero2_DeveRetornarDois()
    {
        var result = _sut.Execute(2);

        Assert.NotNull(result);
        Assert.Equal("Dois", result.NumberAsString);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(-1)]
    [InlineData(99)]
    public void Execute_ComNumeroInvalido_DeveRetornarNull(int number)
    {
        var result = _sut.Execute(number);

        Assert.Null(result);
    }

    [Fact]
    public void Execute_ComNumero1_DeveLogarConversao()
    {
        _sut.Execute(1);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == Microsoft.Extensions.Logging.LogLevel.Information &&
            l.Message.Contains("Iniciar conversao"));

        Assert.Contains(logs, l =>
            l.Level == Microsoft.Extensions.Logging.LogLevel.Information &&
            l.Message.Contains("Conversao concluida"));
    }

    [Fact]
    public void Execute_ComNumeroInvalido_DeveLogarWarning()
    {
        _sut.Execute(99);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == Microsoft.Extensions.Logging.LogLevel.Warning &&
            l.Message.Contains("nao suportado"));
    }
}
