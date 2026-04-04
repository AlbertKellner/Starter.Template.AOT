namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCase(ILogger<NumberStringGetUseCase> logger) : INumberStringGetUseCase
{
    public NumberStringGetOutput? Execute(int value)
    {
        logger.LogInformation("[NumberStringGetUseCase][Execute] Converter número para string. Value={Value}", value);

        var result = value switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (result is null)
        {
            logger.LogWarning("[NumberStringGetUseCase][Execute] Retornar nulo — valor não mapeado. Value={Value}", value);

            return null;
        }

        var output = new NumberStringGetOutput(result);

        logger.LogInformation("[NumberStringGetUseCase][Execute] Retornar string mapeada. Value={Value}, Result={Result}", value, result);

        return output;
    }
}
