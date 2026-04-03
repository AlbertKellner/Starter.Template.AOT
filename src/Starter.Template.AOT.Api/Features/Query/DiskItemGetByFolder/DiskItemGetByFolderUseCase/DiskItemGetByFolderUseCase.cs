namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public class DiskItemGetByFolderUseCase(IDiskItemGetByFolderRepository repository, ILogger<DiskItemGetByFolderUseCase> logger)
{
    public async Task<DiskItemGetByFolderOutput?> ExecuteAsync(DiskItemGetByFolderInput input)
    {
        logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Obter itens da pasta. DriveId={DriveId}, FolderPath={FolderPath}", input.DriveId, input.FolderPath);

        var entity = await repository.ScanFolderAsync(input.DriveId, input.FolderPath);

        if (entity is null)
        {
            logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Pasta não encontrada. DriveId={DriveId}, FolderPath={FolderPath}", input.DriveId, input.FolderPath);

            return null;
        }

        var output = new DiskItemGetByFolderOutput
        {
            DriveId = input.DriveId,
            FolderPath = input.FolderPath,
            Folder = MapToOutput(entity)
        };

        logger.LogInformation("[DiskItemGetByFolderUseCase][ExecuteAsync] Retornar estrutura da pasta. DriveId={DriveId}, FolderPath={FolderPath}", input.DriveId, input.FolderPath);

        return output;
    }

    private static DiskItemFolderOutput MapToOutput(DiskItemGetByFolderEntity entity)
    {
        return new DiskItemFolderOutput
        {
            Name = entity.Name,
            SizeBytes = entity.SizeBytes,
            FormattedSize = FormatBytes(entity.SizeBytes),
            IsFolder = entity.IsFolder,
            Extension = entity.Extension,
            Children = entity.Children.Select(MapToOutput).ToList()
        };
    }

    private static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        double len = bytes;
        var order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
