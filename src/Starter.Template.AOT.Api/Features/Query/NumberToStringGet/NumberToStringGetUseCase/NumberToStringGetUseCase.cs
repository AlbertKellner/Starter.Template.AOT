namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

public sealed class NumberToStringGetUseCase(ILogger<NumberToStringGetUseCase> logger) : INumberToStringGetUseCase
{
    public NumberToStringGetOutput Execute(int number)
    {
        logger.LogInformation("[NumberToStringGetUseCase][Execute] Processar conversão de número para string. Number={Number}", number);

        var result = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Número não suportado. Valores aceitos: 1, 2.")
        };

        logger.LogInformation("[NumberToStringGetUseCase][Execute] Retornar resultado da conversão. Result={Result}", result);

        return new NumberToStringGetOutput(result);
    }
}
