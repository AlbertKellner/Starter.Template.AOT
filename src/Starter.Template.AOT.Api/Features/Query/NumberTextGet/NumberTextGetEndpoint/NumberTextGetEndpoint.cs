using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberTextGet;

[ApiController]
[Route("number-text")]
public class NumberTextGetEndpoint(INumberTextGetUseCase useCase, ILogger<NumberTextGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Get(int number)
    {
        logger.LogInformation("[NumberTextGetEndpoint][Get] Receber requisição para número {Number}", number);

        var output = useCase.Execute(number);

        logger.LogInformation("[NumberTextGetEndpoint][Get] Retornar resposta com valor {Value}", output.Value);

        return Ok(output);
    }
}
