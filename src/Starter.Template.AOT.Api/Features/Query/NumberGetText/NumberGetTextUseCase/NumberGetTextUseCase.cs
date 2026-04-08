namespace Starter.Template.AOT.Api.Features.Query.NumberGetText;

public class NumberGetTextUseCase(ILogger<NumberGetTextUseCase> logger) : INumberGetTextUseCase
{
    public NumberGetTextOutput? Execute(int number)
    {
        logger.LogInformation("[NumberGetTextUseCase][Execute] Processar número recebido. Number={Number}", number);

        var text = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (text is null)
        {
            logger.LogInformation("[NumberGetTextUseCase][Execute] Retornar null — number={Number} não mapeado", number);
            return null;
        }

        logger.LogInformation("[NumberGetTextUseCase][Execute] Retornar text. Text={Text}", text);

        return new NumberGetTextOutput(text);
    }
}
