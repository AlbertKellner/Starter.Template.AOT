using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

[ApiController]
[Route("number-strings")]
public sealed class NumberStringGetEndpoint(INumberStringGetUseCase useCase, ILogger<NumberStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{value:int}")]
    public IActionResult Get([FromRoute] int value)
    {

        logger.LogInformation("[NumberStringGetEndpoint][Get] Converter número para string. Value={Value}", value);

        var output = useCase.Execute(value);

        if (output is null)
        {

            logger.LogWarning("[NumberStringGetEndpoint][Get] Retornar 400 — valor não suportado. Value={Value}", value);

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Unsupported value",
                Detail = $"Value '{value}' is not supported. Use 1 or 2.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            });
        }

        logger.LogInformation("[NumberStringGetEndpoint][Get] Retornar 200 com string mapeada. Value={Value}, Result={Result}", value, output.Value);

        return Ok(output);
    }
}
