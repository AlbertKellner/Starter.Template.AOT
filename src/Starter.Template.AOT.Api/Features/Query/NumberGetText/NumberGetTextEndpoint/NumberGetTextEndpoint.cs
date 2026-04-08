using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.NumberGetText;

[ApiController]
[Route("number-texts")]
public class NumberGetTextEndpoint(INumberGetTextUseCase useCase, ILogger<NumberGetTextEndpoint> logger) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult GetText(int number)
    {
        logger.LogInformation("[NumberGetTextEndpoint][GetText] Processar requisição. Number={Number}", number);

        var output = useCase.Execute(number);

        if (output is null)
        {
            logger.LogInformation("[NumberGetTextEndpoint][GetText] Retornar 404 — number={Number} não mapeado", number);
            return NotFound();
        }

        logger.LogInformation("[NumberGetTextEndpoint][GetText] Retornar resposta. Text={Text}", output.Text);

        return Ok(output);
    }
}
