namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public class NumberStringGetUseCase(ILogger<NumberStringGetUseCase> logger) : INumberStringGetUseCase
{
    public NumberStringGetOutput Execute(int number)
    {
        logger.LogInformation("[NumberStringGetUseCase][Execute] Processar conversão do número {Number} para string", number);

        var value = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Apenas os valores 1 e 2 são aceitos")
        };

        logger.LogInformation("[NumberStringGetUseCase][Execute] Retornar valor convertido: {Value}", value);

        return new NumberStringGetOutput(value);
    }
}
