using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByFolder;

[ApiController]
[Route("disk-structure")]
public class DiskStructureGetByFolderEndpoint(
    IDiskStructureGetByFolderUseCase useCase,
    ILogger<DiskStructureGetByFolderEndpoint> logger) : ControllerBase
{
    [HttpGet("{selectedDrive}/folder/{selectedFolder}")]
    [ProducesResponseType(typeof(DiskStructureGetByFolderOutput), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetByFolder(
        [FromRoute] string selectedDrive,
        [FromRoute] string selectedFolder)
    {
        logger.LogInformation("[DiskStructureGetByFolderEndpoint][GetByFolder] Receber requisição para buscar pasta {Folder} no drive {Drive}", selectedFolder, selectedDrive);

        var output = await useCase.ExecuteAsync(selectedDrive, selectedFolder);

        if (output is null)
        {
            logger.LogInformation("[DiskStructureGetByFolderEndpoint][GetByFolder] Retornar 204 — pasta não encontrada");

            return NoContent();
        }

        logger.LogInformation("[DiskStructureGetByFolderEndpoint][GetByFolder] Retornar 200 com estrutura da pasta {Folder}", selectedFolder);

        return Ok(output.Structure);
    }
}
