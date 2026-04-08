using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

[ApiController]
[Route("number-to-string")]
public sealed class NumberToStringGetEndpoint(INumberToStringGetUseCase useCase, ILogger<NumberToStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get(int number)
    {

        logger.LogInformation("[NumberToStringGetEndpoint][Get] Receber requisição. Number={Number}", number);

        var result = useCase.Execute(number);

        logger.LogInformation("[NumberToStringGetEndpoint][Get] Retornar resultado. Result={Result}", result);

        return Ok(result);
    }
}
