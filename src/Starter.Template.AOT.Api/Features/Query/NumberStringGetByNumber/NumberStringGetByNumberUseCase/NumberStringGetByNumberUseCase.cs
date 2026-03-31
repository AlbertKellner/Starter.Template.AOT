namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;

public sealed class NumberStringGetByNumberUseCase(
    ILogger<NumberStringGetByNumberUseCase> logger) : INumberStringGetByNumberUseCase
{
    public NumberStringGetByNumberOutput? Execute(int number)
    {
        logger.LogInformation("[NumberStringGetByNumberUseCase][Execute] Iniciar conversao do numero {Number}", number);

        var result = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (result is null)
        {
            logger.LogWarning("[NumberStringGetByNumberUseCase][Execute] Numero {Number} nao suportado", number);

            return null;
        }

        logger.LogInformation("[NumberStringGetByNumberUseCase][Execute] Conversao concluida: {Number} -> {Result}", number, result);

        return new NumberStringGetByNumberOutput(result);
    }
}
