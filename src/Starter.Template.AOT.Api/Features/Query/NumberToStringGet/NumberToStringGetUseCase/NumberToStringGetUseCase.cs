namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

public sealed class NumberToStringGetUseCase(ILogger<NumberToStringGetUseCase> logger) : INumberToStringGetUseCase
{
    public NumberToStringGetOutput? Execute(int number)
    {
        logger.LogInformation("[NumberToStringGetUseCase][Execute] Converter número para string. Number={Number}", number);

        var result = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (result is null)
        {
            logger.LogInformation("[NumberToStringGetUseCase][Execute] Retornar nulo — número não mapeado. Number={Number}", number);

            return null;
        }

        logger.LogInformation("[NumberToStringGetUseCase][Execute] Retornar resultado. Number={Number}, Value={Value}", number, result);

        return new NumberToStringGetOutput(result);
    }
}
