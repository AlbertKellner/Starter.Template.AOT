namespace Starter.Template.AOT.Api.Features.Query.NumberGetByValue;

public class NumberGetByValueUseCase(ILogger<NumberGetByValueUseCase> logger) : INumberGetByValueUseCase
{
    public NumberGetByValueOutput Execute(int number)
    {

        logger.LogInformation("[NumberGetByValueUseCase][Execute] Processar conversão do número {Number} para string", number);

        var value = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Valor não suportado. Valores aceitos: 1, 2.")
        };

        logger.LogInformation("[NumberGetByValueUseCase][Execute] Retornar valor convertido: {Value}", value);

        return new NumberGetByValueOutput(value);
    }
}
