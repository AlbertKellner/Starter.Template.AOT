namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

public class DiskDrivesGetAllRepository(ILogger<DiskDrivesGetAllRepository> logger) : IDiskDrivesGetAllRepository
{
    public DriveInfo[] GetAllDrives()
    {
        logger.LogInformation("[DiskDrivesGetAllRepository][GetAllDrives] Obter todas as unidades de disco disponíveis");

        var drives = DriveInfo.GetDrives();

        logger.LogInformation("[DiskDrivesGetAllRepository][GetAllDrives] Retornar {Count} unidades encontradas", drives.Length);

        return drives;
    }
}
