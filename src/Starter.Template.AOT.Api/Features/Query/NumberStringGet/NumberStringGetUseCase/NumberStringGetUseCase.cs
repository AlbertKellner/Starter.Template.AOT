namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public class NumberStringGetUseCase(ILogger<NumberStringGetUseCase> logger) : INumberStringGetUseCase
{
    public NumberStringGetOutput Execute(int number)
    {
        logger.LogInformation("[NumberStringGetUseCase][Execute] Processar conversão do número {Number} para string", number);

        var value = number switch
        {
            0 => "Zero",
            1 => "Um",
            2 => "Dois",
            3 => "Três",
            4 => "Quatro",
            5 => "Cinco",
            6 => "Seis",
            7 => "Sete",
            8 => "Oito",
            9 => "Nove",
            10 => "Dez",
            _ => throw new ArgumentOutOfRangeException(nameof(number), number, "Apenas os valores de 0 a 10 são aceitos")
        };

        logger.LogInformation("[NumberStringGetUseCase][Execute] Retornar valor convertido: {Value}", value);

        return new NumberStringGetOutput(value);
    }
}
