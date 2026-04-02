namespace Starter.Template.AOT.Api.Features.Query.NumberToString;

public sealed class NumberToStringUseCase(ILogger<NumberToStringUseCase> logger) : INumberToStringUseCase
{
    public Task<NumberToStringOutput?> ExecuteAsync(int number)
    {
        logger.LogInformation("[NumberToStringUseCase][ExecuteAsync] Converter número para string: {Number}", number);

        var value = number switch
        {
            1 => "Um",
            2 => "Dois",
            3 => "Três",
            4 => "Quatro",
            5 => "Cinco",
            _ => null
        };

        if (value is null)
        {
            logger.LogInformation("[NumberToStringUseCase][ExecuteAsync] Retornar null — número não mapeado: {Number}", number);

            return Task.FromResult<NumberToStringOutput?>(null);
        }

        var output = new NumberToStringOutput { Value = value };

        logger.LogInformation("[NumberToStringUseCase][ExecuteAsync] Retornar conversão: {Number} -> {Value}", number, value);

        return Task.FromResult<NumberToStringOutput?>(output);
    }
}
