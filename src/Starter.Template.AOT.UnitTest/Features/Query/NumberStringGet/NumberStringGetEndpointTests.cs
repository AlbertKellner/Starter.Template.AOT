using Starter.Template.AOT.Api.Features.Query.NumberStringGet;
using Starter.Template.AOT.UnitTest.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberStringGet;

public sealed class NumberStringGetEndpointTests
{
    private sealed class StubNumberStringGetUseCase(NumberStringGetOutput output) : INumberStringGetUseCase
    {
        public NumberStringGetOutput Execute(int value) => output;
    }

    [Fact]
    public void Get_DeveRetornarOkComOutputCorreto()
    {
        var expectedOutput = new NumberStringGetOutput(1, "Um");
        var stubUseCase = new StubNumberStringGetUseCase(expectedOutput);
        var fakeEndpointLogger = new FakeLogger<NumberStringGetEndpoint>();
        var endpoint = new NumberStringGetEndpoint(stubUseCase, fakeEndpointLogger);

        var result = endpoint.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var output = Assert.IsType<NumberStringGetOutput>(okResult.Value);
        Assert.Equal(1, output.Value);
        Assert.Equal("Um", output.Text);
    }

    [Fact]
    public void Get_DeveRegistrarLogDeRecebimentoDeRequisicao()
    {
        var expectedOutput = new NumberStringGetOutput(1, "Um");
        var stubUseCase = new StubNumberStringGetUseCase(expectedOutput);
        var fakeEndpointLogger = new FakeLogger<NumberStringGetEndpoint>();
        var endpoint = new NumberStringGetEndpoint(stubUseCase, fakeEndpointLogger);

        endpoint.Get(1);

        var logs = fakeEndpointLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Receber"));
    }

    [Fact]
    public void Get_DeveRegistrarLogDeRetorno()
    {
        var expectedOutput = new NumberStringGetOutput(2, "Dois");
        var stubUseCase = new StubNumberStringGetUseCase(expectedOutput);
        var fakeEndpointLogger = new FakeLogger<NumberStringGetEndpoint>();
        var endpoint = new NumberStringGetEndpoint(stubUseCase, fakeEndpointLogger);

        endpoint.Get(2);

        var logs = fakeEndpointLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }
}
