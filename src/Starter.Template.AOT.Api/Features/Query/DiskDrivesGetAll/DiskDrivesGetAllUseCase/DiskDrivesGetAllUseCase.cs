using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

public class DiskDrivesGetAllUseCase(
    IDiskScannerService diskScannerService,
    ILogger<DiskDrivesGetAllUseCase> logger) : IDiskDrivesGetAllUseCase
{
    public DiskDrivesGetAllOutput Execute()
    {
        logger.LogInformation("[DiskDrivesGetAllUseCase][Execute] Obter lista de drives disponíveis");

        var driveNames = diskScannerService.GetDriveNames();

        logger.LogInformation("[DiskDrivesGetAllUseCase][Execute] Retornar {Count} drives", driveNames.Count);

        return new DiskDrivesGetAllOutput(driveNames);
    }
}
