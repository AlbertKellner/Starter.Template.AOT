using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

[ApiController]
[Route("number-to-string")]
public class NumberToStringGetEndpoint(INumberToStringGetUseCase useCase, ILogger<NumberToStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get(int number)
    {
        logger.LogInformation("[NumberToStringGetEndpoint][Get] Receber requisição. Number={Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {
            logger.LogInformation("[NumberToStringGetEndpoint][Get] Retornar 404 — número não mapeado. Number={Number}", number);

            return NotFound();
        }

        logger.LogInformation("[NumberToStringGetEndpoint][Get] Retornar resultado. Number={Number}, Value={Value}", number, output.Value);

        return Ok(output);
    }
}
