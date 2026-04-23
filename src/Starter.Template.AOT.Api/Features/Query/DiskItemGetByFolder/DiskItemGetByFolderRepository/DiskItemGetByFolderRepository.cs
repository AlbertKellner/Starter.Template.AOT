using Starter.Template.AOT.Api.Shared.DiskItem;
using Starter.Template.AOT.Api.Shared.FileSystem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public class DiskItemGetByFolderRepository(ILogger<DiskItemGetByFolderRepository> logger) : IDiskItemGetByFolderRepository
{
    public async Task<DiskItemEntity?> GetAsync(int driveIndex, string folderPath)
    {
        logger.LogInformation("[DiskItemGetByFolderRepository][GetAsync] Escanear unidade {DriveIndex} para pasta {FolderPath}", driveIndex, folderPath);

        var drives = DriveInfo.GetDrives();

        if (driveIndex < 0 || driveIndex >= drives.Length)
            throw new ArgumentOutOfRangeException(nameof(driveIndex), $"Índice {driveIndex} inválido. Total de unidades: {drives.Length}");

        var drive = drives[driveIndex];

        var root = await FileSystemExplorer.ScanAsync(drive.Name);

        logger.LogInformation("[DiskItemGetByFolderRepository][GetAsync] Localizar pasta no caminho {FolderPath}", folderPath);

        var segments = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var folder = root.FindFolder(segments);

        logger.LogInformation("[DiskItemGetByFolderRepository][GetAsync] Retornar pasta {Found}", folder != null ? "encontrada" : "não encontrada");

        return folder;
    }
}
