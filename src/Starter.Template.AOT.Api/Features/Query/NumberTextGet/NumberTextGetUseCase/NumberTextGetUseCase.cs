namespace Starter.Template.AOT.Api.Features.Query.NumberTextGet;

public class NumberTextGetUseCase(ILogger<NumberTextGetUseCase> logger) : INumberTextGetUseCase
{
    public NumberTextGetOutput Execute(int number)
    {
        logger.LogInformation("[NumberTextGetUseCase][Execute] Processar conversão do número {Number} para texto", number);

        var value = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Apenas os valores 1 e 2 são aceitos")
        };

        logger.LogInformation("[NumberTextGetUseCase][Execute] Retornar valor convertido: {Value}", value);

        return new NumberTextGetOutput(value);
    }
}
