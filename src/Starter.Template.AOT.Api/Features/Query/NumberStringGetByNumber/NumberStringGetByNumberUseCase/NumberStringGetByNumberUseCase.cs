namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberUseCase(ILogger<NumberStringGetByNumberUseCase> logger) : INumberStringGetByNumberUseCase
{
    public NumberStringGetByNumberOutput? Execute(int number)
    {
        logger.LogInformation("[NumberStringGetByNumberUseCase][Execute] Converter número {Number} para string", number);

        var result = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (result is null)
        {
            logger.LogWarning("[NumberStringGetByNumberUseCase][Execute] Número {Number} não possui mapeamento definido", number);

            return null;
        }

        logger.LogInformation("[NumberStringGetByNumberUseCase][Execute] Retornar valor '{Value}' para número {Number}", result, number);

        return new NumberStringGetByNumberOutput(result);
    }
}
