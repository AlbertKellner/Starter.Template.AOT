namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

public interface INumberToStringGetUseCase
{
    string Execute(int number);
}

public sealed class NumberToStringGetUseCase(ILogger<NumberToStringGetUseCase> logger) : INumberToStringGetUseCase
{
    public string Execute(int number)
    {

        logger.LogInformation("[NumberToStringGetUseCase][Execute] Converter número para string. Number={Number}", number);

        var result = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => throw new ArgumentOutOfRangeException(nameof(number), $"Número {number} não suportado.")
        };

        logger.LogInformation("[NumberToStringGetUseCase][Execute] Retornar resultado. Result={Result}", result);

        return result;
    }
}
