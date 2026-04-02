using System.Collections.Frozen;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public sealed class NumberStringGetUseCase(ILogger<NumberStringGetUseCase> logger) : INumberStringGetUseCase
{
    private static readonly FrozenDictionary<int, string> NumberMap = new Dictionary<int, string>
    {
        { 1, "Um" },
        { 2, "Dois" }
    }.ToFrozenDictionary();

    public NumberStringGetOutput Execute(int value)
    {
        logger.LogInformation("[NumberStringGetUseCase][Execute] Converter valor numérico para texto. Value={Value}", value);

        if (!NumberMap.TryGetValue(value, out var text))
        {
            logger.LogWarning("[NumberStringGetUseCase][Execute] Valor não mapeado. Value={Value}", value);

            throw new KeyNotFoundException($"Value {value} is not mapped to a string representation.");
        }

        logger.LogInformation("[NumberStringGetUseCase][Execute] Retornar texto correspondente. Value={Value}, Text={Text}", value, text);

        return new NumberStringGetOutput(value, text);
    }
}
