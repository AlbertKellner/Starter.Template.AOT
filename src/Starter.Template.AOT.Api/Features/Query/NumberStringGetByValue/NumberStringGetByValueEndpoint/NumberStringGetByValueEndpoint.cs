using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;

[ApiController]
[Route("number-string")]
public sealed class NumberStringGetByValueEndpoint(
    INumberStringGetByValueUseCase useCase,
    ILogger<NumberStringGetByValueEndpoint> logger) : ControllerBase
{
    [HttpGet("{value:int}")]
    public IActionResult GetByValue(int value)
    {
        logger.LogInformation("[NumberStringGetByValueEndpoint][GetByValue] Receber requisição. Value={Value}", value);

        var output = useCase.Execute(value);

        if (output is null)
        {
            logger.LogWarning("[NumberStringGetByValueEndpoint][GetByValue] Retornar 404 — número não encontrado. Value={Value}", value);

            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Number not found",
                Detail = $"No string representation found for value {value}.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
            });
        }

        logger.LogInformation("[NumberStringGetByValueEndpoint][GetByValue] Retornar resultado. Value={Value}, Text={Text}", value, output.Value);

        return Ok(output);
    }
}
