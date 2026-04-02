using Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;
using Starter.Template.AOT.UnitTest.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByValue;

public sealed class NumberStringGetByValueEndpointTests
{
    private sealed class FakeUseCase(NumberStringGetByValueOutput? result) : INumberStringGetByValueUseCase
    {
        public NumberStringGetByValueOutput? Execute(int value) => result;
    }

    [Fact]
    public void GetByValue_ComValorMapeado_DeveRetornarOk()
    {
        var logger = new FakeLogger<NumberStringGetByValueEndpoint>();
        var useCase = new FakeUseCase(new NumberStringGetByValueOutput("Um"));
        var endpoint = new NumberStringGetByValueEndpoint(useCase, logger);

        var result = endpoint.GetByValue(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var output = Assert.IsType<NumberStringGetByValueOutput>(okResult.Value);
        Assert.Equal("Um", output.Value);
    }

    [Fact]
    public void GetByValue_ComValorNaoMapeado_DeveRetornarNotFound()
    {
        var logger = new FakeLogger<NumberStringGetByValueEndpoint>();
        var useCase = new FakeUseCase(null);
        var endpoint = new NumberStringGetByValueEndpoint(useCase, logger);

        var result = endpoint.GetByValue(99);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.IsType<ProblemDetails>(notFoundResult.Value);
    }

    [Fact]
    public void GetByValue_ComValorMapeado_DeveRegistrarLogDeRequisicaoERetorno()
    {
        var logger = new FakeLogger<NumberStringGetByValueEndpoint>();
        var useCase = new FakeUseCase(new NumberStringGetByValueOutput("Dois"));
        var endpoint = new NumberStringGetByValueEndpoint(useCase, logger);

        endpoint.GetByValue(2);

        var logs = logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Receber requisição"));

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar resultado"));
    }

    [Fact]
    public void GetByValue_ComValorNaoMapeado_DeveRegistrarLogWarning()
    {
        var logger = new FakeLogger<NumberStringGetByValueEndpoint>();
        var useCase = new FakeUseCase(null);
        var endpoint = new NumberStringGetByValueEndpoint(useCase, logger);

        endpoint.GetByValue(3);

        var logs = logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == LogLevel.Warning &&
            l.Message.Contains("número não encontrado"));
    }
}
