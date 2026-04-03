namespace Starter.Template.AOT.Api.Features.Query.DrivesGetAll;

public class DrivesGetAllUseCase(IDrivesGetAllRepository repository, ILogger<DrivesGetAllUseCase> logger)
{
    public DrivesGetAllOutput Execute()
    {
        logger.LogInformation("[DrivesGetAllUseCase][Execute] Obter todos os drives disponíveis");

        var entities = repository.GetAllDrives();

        var drives = entities.Select(e => new DriveOutput
        {
            Id = e.Id,
            Name = e.Name,
            DriveType = e.DriveType,
            TotalSizeBytes = e.TotalSizeBytes,
            AvailableSizeBytes = e.AvailableSizeBytes,
            FormattedTotalSize = FormatBytes(e.TotalSizeBytes),
            FormattedAvailableSize = FormatBytes(e.AvailableSizeBytes)
        }).ToList();

        var output = new DrivesGetAllOutput { Drives = drives };

        logger.LogInformation("[DrivesGetAllUseCase][Execute] Retornar {Count} drives formatados", output.Drives.Count);

        return output;
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
