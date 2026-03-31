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
        logger.LogInformation("[NumberStringGetByNumberEndpoint][GetByNumber] Receber requisicao para numero {Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {
            logger.LogWarning("[NumberStringGetByNumberEndpoint][GetByNumber] Retornar 400 - numero {Number} invalido", number);

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = $"The number {number} is not supported. Only 1 and 2 are valid.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            });
        }

        logger.LogInformation("[NumberStringGetByNumberEndpoint][GetByNumber] Retornar 200 com resultado {Result}", output.NumberAsString);

        return Ok(output);
    }
}
