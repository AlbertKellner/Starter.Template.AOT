namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCase(ILogger<NumberStringGetUseCase> logger) : INumberStringGetUseCase
{
    private static readonly Dictionary<int, string> NumberMap = new()
    {
        { 1, "Um" },
        { 2, "Dois" }
    };

    public NumberStringGetOutput? Execute(int number)
    {
        logger.LogInformation("[NumberStringGetUseCase][Execute] Converter número {Number} para string", number);

        if (!NumberMap.TryGetValue(number, out var value))
        {
            logger.LogWarning("[NumberStringGetUseCase][Execute] Número {Number} não possui mapeamento", number);

            return null;
        }

        logger.LogInformation("[NumberStringGetUseCase][Execute] Retornar valor \"{Value}\" para número {Number}", value, number);

        return new NumberStringGetOutput(value);
    }
}
