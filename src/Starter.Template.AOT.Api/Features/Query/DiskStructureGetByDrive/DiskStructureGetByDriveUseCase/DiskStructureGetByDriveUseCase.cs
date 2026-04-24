using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByDrive;

public class DiskStructureGetByDriveUseCase(
    IDiskScannerService diskScannerService,
    ILogger<DiskStructureGetByDriveUseCase> logger) : IDiskStructureGetByDriveUseCase
{
    public async Task<DiskStructureGetByDriveOutput> ExecuteAsync(string selectedDrive)
    {
        logger.LogInformation("[DiskStructureGetByDriveUseCase][ExecuteAsync] Escanear drive {Drive}", selectedDrive);

        var drivePath = $"{selectedDrive}:/";
        var structure = await diskScannerService.ScanDriveAsync(drivePath);

        logger.LogInformation("[DiskStructureGetByDriveUseCase][ExecuteAsync] Aplicar cores à estrutura");

        ColorGenerator.ApplyColorsToStructure(structure);

        logger.LogInformation("[DiskStructureGetByDriveUseCase][ExecuteAsync] Retornar estrutura do drive {Drive}", selectedDrive);

        return new DiskStructureGetByDriveOutput(structure);
    }
}
