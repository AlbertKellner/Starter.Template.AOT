namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;

public sealed class NumberStringGetByValueUseCase(ILogger<NumberStringGetByValueUseCase> logger) : INumberStringGetByValueUseCase
{
    private static readonly Dictionary<int, string> NumberMap = new()
    {
        { 1, "Um" },
        { 2, "Dois" }
    };

    public NumberStringGetByValueOutput? Execute(int value)
    {
        logger.LogInformation("[NumberStringGetByValueUseCase][Execute] Buscar representação textual do número. Value={Value}", value);

        if (NumberMap.TryGetValue(value, out var text))
        {
            logger.LogInformation("[NumberStringGetByValueUseCase][Execute] Retornar representação encontrada. Value={Value}, Text={Text}", value, text);

            return new NumberStringGetByValueOutput(text);
        }

        logger.LogWarning("[NumberStringGetByValueUseCase][Execute] Retornar nulo — número não mapeado. Value={Value}", value);

        return null;
    }
}
