using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByDrive;

[ApiController]
[Route("disk-structure")]
public class DiskStructureGetByDriveEndpoint(
    IDiskStructureGetByDriveUseCase useCase,
    ILogger<DiskStructureGetByDriveEndpoint> logger) : ControllerBase
{
    [HttpGet("{selectedDrive}")]
    [ProducesResponseType(typeof(DiskStructureGetByDriveOutput), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetByDrive([FromRoute] string selectedDrive)
    {
        logger.LogInformation("[DiskStructureGetByDriveEndpoint][GetByDrive] Receber requisição para escanear drive {Drive}", selectedDrive);

        var output = await useCase.ExecuteAsync(selectedDrive);

        if (output.Structure.Children is null || output.Structure.Children.Count == 0)
        {
            logger.LogInformation("[DiskStructureGetByDriveEndpoint][GetByDrive] Retornar 204 — drive vazio ou inacessível");

            return NoContent();
        }

        logger.LogInformation("[DiskStructureGetByDriveEndpoint][GetByDrive] Retornar 200 com estrutura do drive {Drive}", selectedDrive);

        return Ok(output.Structure);
    }
}
