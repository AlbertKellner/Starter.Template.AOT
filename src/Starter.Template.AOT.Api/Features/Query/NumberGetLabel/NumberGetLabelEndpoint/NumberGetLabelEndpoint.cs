using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberGetLabel;

[ApiController]
[Route("numbers")]
public class NumberGetLabelEndpoint(INumberGetLabelUseCase useCase, ILogger<NumberGetLabelEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult GetLabel(int number)
    {

        logger.LogInformation("[NumberGetLabelEndpoint][GetLabel] Processar requisição. Number={Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {

            logger.LogInformation("[NumberGetLabelEndpoint][GetLabel] Retornar 404 — number={Number} não mapeado", number);

            return NotFound();
        }

        logger.LogInformation("[NumberGetLabelEndpoint][GetLabel] Retornar resposta. Label={Label}", output.Label);

        return Ok(output);
    }
}
