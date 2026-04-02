using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;

[ApiController]
[Route("number-string")]
public sealed class NumberStringGetByNumberEndpoint(
    INumberStringGetByNumberUseCase useCase,
    ILogger<NumberStringGetByNumberEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult GetByNumber(int number)
    {
        logger.LogInformation("[NumberStringGetByNumberEndpoint][GetByNumber] Receber requisição para número {Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {
            logger.LogWarning("[NumberStringGetByNumberEndpoint][GetByNumber] Número {Number} não encontrado — retornar 404", number);

            return NotFound();
        }

        logger.LogInformation("[NumberStringGetByNumberEndpoint][GetByNumber] Retornar valor '{Value}' com status 200", output.Value);

        return Ok(output);
    }
}
