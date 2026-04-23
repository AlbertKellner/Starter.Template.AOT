using Starter.Template.AOT.Api.Shared.DiskItem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public class DiskItemGetByFolderUseCase(
    IDiskItemGetByFolderRepository repository,
    ILogger<DiskItemGetByFolderUseCase> logger) : IDiskItemGetByFolderUseCase
{
    public async Task<DiskItemGetByFolderOutput?> ExecuteAsync(int driveIndex, string folderPath)
    {
        logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Buscar pasta {FolderPath} na unidade {DriveIndex}", folderPath, driveIndex);

        var entity = await repository.GetAsync(driveIndex, folderPath);

        if (entity == null)
        {
            logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Pasta {FolderPath} não encontrada", folderPath);
            return null;
        }

        logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Mapear pasta {FolderPath} com {Count} filhos", folderPath, entity.Children.Count);

        var output = MapToOutput(entity);

        logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Retornar output da pasta {FolderPath}", folderPath);

        return output;
    }

    private static DiskItemGetByFolderOutput MapToOutput(DiskItemEntity entity) =>
        new()
        {
            Name = entity.Name,
            Value = entity.Size,
            FormattedSize = entity.FormattedSize,
            Children = entity.Children.Count > 0
                ? entity.Children.Select(MapToOutput).ToList()
                : null
        };
}
