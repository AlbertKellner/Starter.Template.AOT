using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

[ApiController]
[Route("number-string")]
public class NumberStringGetEndpoint(INumberStringGetUseCase useCase, ILogger<NumberStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get([FromRoute] int number)
    {
        logger.LogInformation("[NumberStringGetEndpoint][Get] Receber requisição para número {Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {
            logger.LogWarning("[NumberStringGetEndpoint][Get] Número {Number} não encontrado — retornar 404", number);

            return NotFound();
        }

        logger.LogInformation("[NumberStringGetEndpoint][Get] Retornar valor \"{Value}\" com status 200", output.Value);

        return Ok(output);
    }
}
