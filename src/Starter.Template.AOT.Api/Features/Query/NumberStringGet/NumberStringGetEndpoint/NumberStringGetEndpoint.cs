using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

[ApiController]
[Route("number-string")]
public class NumberStringGetEndpoint(INumberStringGetUseCase useCase, ILogger<NumberStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get(int number)
    {
        logger.LogInformation("[NumberStringGetEndpoint][Get] Receber requisição para número {Number}", number);

        var output = useCase.Execute(number);

        logger.LogInformation("[NumberStringGetEndpoint][Get] Retornar resposta com valor {Value}", output.Value);

        return Ok(output);
    }
}
