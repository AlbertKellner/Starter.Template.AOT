using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByFolder;

public class DiskStructureGetByFolderUseCase(
    IDiskScannerService diskScannerService,
    ILogger<DiskStructureGetByFolderUseCase> logger) : IDiskStructureGetByFolderUseCase
{
    public async Task<DiskStructureGetByFolderOutput?> ExecuteAsync(string selectedDrive, string selectedFolder)
    {
        logger.LogInformation("[DiskStructureGetByFolderUseCase][ExecuteAsync] Escanear drive {Drive} e buscar pasta {Folder}", selectedDrive, selectedFolder);

        var drivePath = $"{selectedDrive}:/";
        var structure = await diskScannerService.ScanDriveAsync(drivePath);

        logger.LogInformation("[DiskStructureGetByFolderUseCase][ExecuteAsync] Buscar pasta {Folder} na estrutura escaneada", selectedFolder);

        var result = diskScannerService.FindFolder(structure, selectedFolder);

        if (result is null)
        {
            logger.LogInformation("[DiskStructureGetByFolderUseCase][ExecuteAsync] Pasta {Folder} não encontrada", selectedFolder);

            return null;
        }

        logger.LogInformation("[DiskStructureGetByFolderUseCase][ExecuteAsync] Retornar estrutura da pasta {Folder}", selectedFolder);

        return new DiskStructureGetByFolderOutput(result);
    }
}
