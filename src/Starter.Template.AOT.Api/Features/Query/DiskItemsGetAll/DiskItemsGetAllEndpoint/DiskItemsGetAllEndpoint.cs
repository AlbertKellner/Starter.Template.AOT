using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

[ApiController]
[Route("disk-items")]
public class DiskItemsGetAllEndpoint(
    IDiskItemsGetAllUseCase useCase,
    ILogger<DiskItemsGetAllEndpoint> logger) : ControllerBase
{
    [HttpGet("{driveIndex:int}")]
    public async Task<IActionResult> GetAll(int driveIndex)
    {
        logger.LogInformation("[DiskItemsGetAllEndpoint][GetAll] Receber requisição para unidade de índice {DriveIndex}", driveIndex);

        var output = await useCase.ExecuteAsync(driveIndex);

        logger.LogInformation("[DiskItemsGetAllEndpoint][GetAll] Retornar estrutura da unidade {DriveIndex}", driveIndex);

        return Ok(output);
    }
}
