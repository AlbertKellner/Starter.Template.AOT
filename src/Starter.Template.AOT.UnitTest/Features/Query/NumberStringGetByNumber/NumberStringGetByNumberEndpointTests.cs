using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberEndpointTests
{
    private readonly FakeLogger<NumberStringGetByNumberEndpoint> _logger = new();

    [Fact]
    public void GetByNumber_DeveRetornarOk_QuandoNumeroEncontrado()
    {
        var useCase = new StubUseCase(new NumberStringGetByNumberOutput("Um"));
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, _logger);

        var result = endpoint.GetByNumber(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var output = Assert.IsType<NumberStringGetByNumberOutput>(okResult.Value);
        Assert.Equal("Um", output.Value);
    }

    [Fact]
    public void GetByNumber_DeveRetornarNotFound_QuandoNumeroNaoMapeado()
    {
        var useCase = new StubUseCase(null);
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, _logger);

        var result = endpoint.GetByNumber(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetByNumber_DeveRegistrarLogInformation_QuandoNumeroEncontrado()
    {
        var useCase = new StubUseCase(new NumberStringGetByNumberOutput("Um"));
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, _logger);

        endpoint.GetByNumber(1);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Receber requisição"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar valor"));
    }

    [Fact]
    public void GetByNumber_DeveRegistrarLogWarning_QuandoNumeroNaoEncontrado()
    {
        var useCase = new StubUseCase(null);
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, _logger);

        endpoint.GetByNumber(5);

        var logs = _logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("não encontrado"));
    }

    private sealed class StubUseCase(NumberStringGetByNumberOutput? result) : INumberStringGetByNumberUseCase
    {
        public NumberStringGetByNumberOutput? Execute(int number) => result;
    }
}
