namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

public class DiskDrivesGetAllUseCase(
    IDiskDrivesGetAllRepository repository,
    ILogger<DiskDrivesGetAllUseCase> logger) : IDiskDrivesGetAllUseCase
{
    public List<DiskDrivesGetAllOutput> Execute()
    {
        logger.LogInformation("[DiskDrivesGetAllUseCase][Execute] Listar todas as unidades de disco disponíveis");

        var drives = repository.GetAllDrives();

        logger.LogInformation("[DiskDrivesGetAllUseCase][Execute] Encontradas {Count} unidades de disco", drives.Length);

        var output = drives
            .Select((drive, index) => new DiskDrivesGetAllOutput
            {
                Index = index,
                Name = drive.Name,
                DriveType = drive.DriveType.ToString()
            })
            .ToList();

        logger.LogInformation("[DiskDrivesGetAllUseCase][Execute] Retornar {Count} unidades mapeadas", output.Count);

        return output;
    }
}
