using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberToStringGet;

[ApiController]
[Route("number-to-string")]
public class NumberToStringGetEndpoint(INumberToStringGetUseCase useCase, ILogger<NumberToStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get(int number)
    {
        logger.LogInformation("[NumberToStringGetEndpoint][Get] Receber requisição de conversão. Number={Number}", number);

        var output = useCase.Execute(number);

        logger.LogInformation("[NumberToStringGetEndpoint][Get] Retornar resposta da conversão. Result={Result}", output.Result);

        return Ok(output);
    }
}
