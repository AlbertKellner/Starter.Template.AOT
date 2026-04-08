namespace Starter.Template.AOT.Api.Features.Query.NumberGetLabel;

public class NumberGetLabelUseCase(ILogger<NumberGetLabelUseCase> logger) : INumberGetLabelUseCase
{
    public NumberGetLabelOutput? Execute(int number)
    {

        logger.LogInformation("[NumberGetLabelUseCase][Execute] Processar número recebido. Number={Number}", number);

        var label = number switch
        {
            1 => "Um",
            2 => "Dois",
            _ => null
        };

        if (label is null)
        {

            logger.LogInformation("[NumberGetLabelUseCase][Execute] Retornar null — number={Number} não mapeado", number);

            return null;
        }

        logger.LogInformation("[NumberGetLabelUseCase][Execute] Retornar label. Label={Label}", label);

        return new NumberGetLabelOutput(label);
    }
}
