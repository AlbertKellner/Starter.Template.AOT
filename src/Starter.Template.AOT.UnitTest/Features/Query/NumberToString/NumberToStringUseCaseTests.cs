using Microsoft.Extensions.Logging;
using Starter.Template.AOT.Api.Features.Query.NumberToString;
using Starter.Template.AOT.UnitTest.TestHelpers;

namespace Starter.Template.AOT.UnitTest.Features.Query.NumberToString;

public sealed class NumberToStringUseCaseTests
{
    [Theory]
    [InlineData(1, "Um")]
    [InlineData(2, "Dois")]
    [InlineData(3, "Três")]
    [InlineData(4, "Quatro")]
    [InlineData(5, "Cinco")]
    public async Task ExecuteAsync_DeveRetornarTextoCorreto_QuandoNumeroCadastrado(int number, string expected)
    {
        var fakeLogger = new FakeLogger<NumberToStringUseCase>();
        var useCase = new NumberToStringUseCase(fakeLogger);

        var output = await useCase.ExecuteAsync(number);

        Assert.NotNull(output);
        Assert.Equal(expected, output.Value);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNull_QuandoNumeroNaoMapeado()
    {
        var fakeLogger = new FakeLogger<NumberToStringUseCase>();
        var useCase = new NumberToStringUseCase(fakeLogger);

        var output = await useCase.ExecuteAsync(99);

        Assert.Null(output);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRegistrarLogDeConversao_QuandoNumeroCadastrado()
    {
        var fakeLogger = new FakeLogger<NumberToStringUseCase>();
        var useCase = new NumberToStringUseCase(fakeLogger);

        await useCase.ExecuteAsync(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Converter"));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRegistrarLogDeRetorno_QuandoNumeroCadastrado()
    {
        var fakeLogger = new FakeLogger<NumberToStringUseCase>();
        var useCase = new NumberToStringUseCase(fakeLogger);

        await useCase.ExecuteAsync(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.Contains(logs, l =>
            l.Level == LogLevel.Information &&
            l.Message.Contains("Retornar"));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRegistrarLogsComPrefixoCorreto()
    {
        var fakeLogger = new FakeLogger<NumberToStringUseCase>();
        var useCase = new NumberToStringUseCase(fakeLogger);

        await useCase.ExecuteAsync(1);

        var logs = fakeLogger.GetSnapshot();
        Assert.All(logs, l => Assert.Contains("NumberToStringUseCase", l.Message));
    }
}
