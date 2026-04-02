using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

[ApiController]
[Route("number-string")]
public class NumberStringGetEndpoint(INumberStringGetUseCase useCase, ILogger<NumberStringGetEndpoint> logger) : ControllerBase
{
    [HttpGet("{value}")]
    public IActionResult Get(int value)
    {
        logger.LogInformation("[NumberStringGetEndpoint][Get] Receber requisição de conversão número-texto. Value={Value}", value);

        var output = useCase.Execute(value);

        logger.LogInformation("[NumberStringGetEndpoint][Get] Retornar resposta de conversão. Value={Value}, Text={Text}", output.Value, output.Text);

        return Ok(output);
    }
}
