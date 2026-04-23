using Starter.Template.AOT.Api.Shared.DiskItem;
using Starter.Template.AOT.Api.Shared.FileSystem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

public class DiskItemsGetAllRepository(ILogger<DiskItemsGetAllRepository> logger) : IDiskItemsGetAllRepository
{
    public async Task<DiskItemEntity> GetByDriveIndexAsync(int driveIndex)
    {
        logger.LogInformation("[DiskItemsGetAllRepository][GetByDriveIndexAsync] Escanear unidade de índice {DriveIndex}", driveIndex);

        var drives = DriveInfo.GetDrives();

        if (driveIndex < 0 || driveIndex >= drives.Length)
            throw new ArgumentOutOfRangeException(nameof(driveIndex), $"Índice {driveIndex} inválido. Total de unidades: {drives.Length}");

        var drive = drives[driveIndex];

        logger.LogInformation("[DiskItemsGetAllRepository][GetByDriveIndexAsync] Escanear unidade {DriveName}", drive.Name);

        var entity = await FileSystemExplorer.ScanAsync(drive.Name);

        logger.LogInformation("[DiskItemsGetAllRepository][GetByDriveIndexAsync] Retornar estrutura com {Count} itens de nível raiz", entity.Children.Count);

        return entity;
    }
}
