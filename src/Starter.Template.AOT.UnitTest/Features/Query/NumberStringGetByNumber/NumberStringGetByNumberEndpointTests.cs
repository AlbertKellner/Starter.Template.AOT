using Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;
using Starter.Template.AOT.UnitTest.TestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberEndpointTests
{
    private sealed class FakeUseCase(NumberStringGetByNumberOutput? result) : INumberStringGetByNumberUseCase
    {
        public NumberStringGetByNumberOutput? Execute(int number) => result;
    }

    [Fact]
    public void GetByNumber_ComNumeroValido_DeveRetornarOkComOutput()
    {
        var output = new NumberStringGetByNumberOutput("Um");
        var useCase = new FakeUseCase(output);
        var logger = new FakeLogger<NumberStringGetByNumberEndpoint>();
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, logger);

        var result = endpoint.GetByNumber(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOutput = Assert.IsType<NumberStringGetByNumberOutput>(okResult.Value);
        Assert.Equal("Um", returnedOutput.NumberAsString);
    }

    [Fact]
    public void GetByNumber_ComNumeroInvalido_DeveRetornarBadRequest()
    {
        var useCase = new FakeUseCase(null);
        var logger = new FakeLogger<NumberStringGetByNumberEndpoint>();
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, logger);

        var result = endpoint.GetByNumber(99);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<ProblemDetails>(badRequestResult.Value);
    }

    [Fact]
    public void GetByNumber_ComNumeroValido_DeveLogarRequisicao()
    {
        var output = new NumberStringGetByNumberOutput("Dois");
        var useCase = new FakeUseCase(output);
        var logger = new FakeLogger<NumberStringGetByNumberEndpoint>();
        var endpoint = new NumberStringGetByNumberEndpoint(useCase, logger);

        endpoint.GetByNumber(2);

        var logs = logger.GetSnapshot();

        Assert.Contains(logs, l =>
            l.Level == Microsoft.Extensions.Logging.LogLevel.Information &&
            l.Message.Contains("Receber requisicao"));

        Assert.Contains(logs, l =>
            l.Level == Microsoft.Extensions.Logging.LogLevel.Information &&
            l.Message.Contains("Retornar 200"));
    }
}
