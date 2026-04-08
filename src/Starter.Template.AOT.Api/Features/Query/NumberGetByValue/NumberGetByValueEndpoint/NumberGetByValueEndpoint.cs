using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberGetByValue;

[ApiController]
[Route("numbers")]
public class NumberGetByValueEndpoint(INumberGetByValueUseCase useCase, ILogger<NumberGetByValueEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult GetByValue(int number)
    {

        logger.LogInformation("[NumberGetByValueEndpoint][GetByValue] Receber requisição GET /numbers/{Number}", number);

        var output = useCase.Execute(number);

        logger.LogInformation("[NumberGetByValueEndpoint][GetByValue] Retornar resposta com valor: {Value}", output.Value);

        return Ok(output);
    }
}
