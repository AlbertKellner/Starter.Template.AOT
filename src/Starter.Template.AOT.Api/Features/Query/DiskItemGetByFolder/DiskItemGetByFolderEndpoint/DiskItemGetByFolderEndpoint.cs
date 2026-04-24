using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

[ApiController]
[Route("disk-items")]
public class DiskItemGetByFolderEndpoint(
    IDiskItemGetByFolderUseCase useCase,
    ILogger<DiskItemGetByFolderEndpoint> logger) : ControllerBase
{
    [HttpGet("{driveIndex:int}/folder/{*folderPath}")]
    public async Task<IActionResult> GetByFolder(int driveIndex, string folderPath)
    {
        logger.LogInformation("[DiskItemGetByFolderEndpoint][GetByFolder] Receber requisição para pasta {FolderPath} na unidade {DriveIndex}", folderPath, driveIndex);

        var output = await useCase.ExecuteAsync(driveIndex, folderPath);

        if (output == null)
        {
            logger.LogInformation("[DiskItemGetByFolderEndpoint][GetByFolder] Pasta {FolderPath} não encontrada na unidade {DriveIndex}", folderPath, driveIndex);
            return NotFound();
        }

        logger.LogInformation("[DiskItemGetByFolderEndpoint][GetByFolder] Retornar pasta {FolderPath} com {Count} itens", folderPath, output.Children?.Count ?? 0);

        return Ok(output);
    }
}
