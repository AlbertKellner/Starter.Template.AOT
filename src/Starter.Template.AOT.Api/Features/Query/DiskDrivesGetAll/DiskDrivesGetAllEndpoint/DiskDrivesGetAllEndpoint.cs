using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

[ApiController]
[Route("disk-drives")]
public class DiskDrivesGetAllEndpoint(
    IDiskDrivesGetAllUseCase useCase,
    ILogger<DiskDrivesGetAllEndpoint> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        logger.LogInformation("[DiskDrivesGetAllEndpoint][GetAll] Receber requisição para listar unidades de disco");

        var output = useCase.Execute();

        logger.LogInformation("[DiskDrivesGetAllEndpoint][GetAll] Retornar {Count} unidades de disco", output.Count);

        return Ok(output);
    }
}
