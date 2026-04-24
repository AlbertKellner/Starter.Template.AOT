using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

[ApiController]
[Route("disk-drives")]
public class DiskDrivesGetAllEndpoint(
    IDiskDrivesGetAllUseCase useCase,
    ILogger<DiskDrivesGetAllEndpoint> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(DiskDrivesGetAllOutput), 200)]
    [ProducesResponseType(204)]
    public IActionResult GetAll()
    {
        logger.LogInformation("[DiskDrivesGetAllEndpoint][GetAll] Receber requisição para listar drives");

        var output = useCase.Execute();

        if (output.Drives.Count == 0)
        {
            logger.LogInformation("[DiskDrivesGetAllEndpoint][GetAll] Retornar 204 — nenhum drive encontrado");

            return NoContent();
        }

        logger.LogInformation("[DiskDrivesGetAllEndpoint][GetAll] Retornar 200 com {Count} drives", output.Drives.Count);

        return Ok(output);
    }
}
