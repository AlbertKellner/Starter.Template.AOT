using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberToString;

[ApiController]
[Route("number-to-string")]
public sealed class NumberToStringEndpoint(
    INumberToStringUseCase useCase,
    ILogger<NumberToStringEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public async Task<IActionResult> Get([FromRoute] int number)
    {
        logger.LogInformation("[NumberToStringEndpoint][Get] Receber requisição para converter número: {Number}", number);

        var output = await useCase.ExecuteAsync(number);

        if (output is null)
        {
            logger.LogInformation("[NumberToStringEndpoint][Get] Retornar 422 — número não mapeado: {Number}", number);

            return UnprocessableEntity();
        }

        logger.LogInformation("[NumberToStringEndpoint][Get] Retornar resultado: {Number} -> {Value}", number, output.Value);

        return Ok(output);
    }
}
