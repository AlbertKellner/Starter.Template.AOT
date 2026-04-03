namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAllByDrive;

public class DiskItemsGetAllByDriveRepository(ILogger<DiskItemsGetAllByDriveRepository> logger) : IDiskItemsGetAllByDriveRepository
{
    public async Task<DiskItemEntity?> ScanDriveAsync(string driveId)
    {
        logger.LogInformation("[DiskItemsGetAllByDriveRepository][ScanDriveAsync] Iniciar varredura do drive. DriveId={DriveId}", driveId);

        var rootPath = ResolveRootPath(driveId);

        if (rootPath is null || !Directory.Exists(rootPath))
        {
            logger.LogInformation("[DiskItemsGetAllByDriveRepository][ScanDriveAsync] Drive não encontrado. DriveId={DriveId}", driveId);

            return null;
        }

        var root = new DiskItemEntity
        {
            Name = driveId,
            FullPath = rootPath,
            IsFolder = true,
            Children = []
        };

        await ScanDirectoryAsync(root);

        UpdateFolderSize(root);
        SortChildrenBySize(root);

        logger.LogInformation("[DiskItemsGetAllByDriveRepository][ScanDriveAsync] Retornar árvore de itens do drive. DriveId={DriveId}, TotalSizeBytes={Size}", driveId, root.SizeBytes);

        return root;
    }

    private static string? ResolveRootPath(string driveId)
    {
        if (driveId.Equals("root", StringComparison.OrdinalIgnoreCase))
            return "/";

        var candidate = $"{driveId.ToUpperInvariant()}:\\";

        return Directory.Exists(candidate) ? candidate : null;
    }

    private static async Task ScanDirectoryAsync(DiskItemEntity folder)
    {
        var tasks = new List<Task>();

        try
        {
            var subDirectories = Directory.GetDirectories(folder.FullPath);

            foreach (var dir in subDirectories)
            {
                var dirInfo = new DirectoryInfo(dir);
                var subFolder = new DiskItemEntity
                {
                    Name = dirInfo.Name,
                    FullPath = dir,
                    IsFolder = true,
                    Children = []
                };

                folder.Children.Add(subFolder);

                tasks.Add(Task.Run(async () => await ScanDirectoryAsync(subFolder)));
            }

            var files = Directory.GetFiles(folder.FullPath);

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    folder.Children.Add(new DiskItemEntity
                    {
                        Name = fileInfo.Name,
                        FullPath = file,
                        IsFolder = false,
                        Extension = fileInfo.Extension,
                        SizeBytes = fileInfo.Length
                    });
                }
                catch (IOException)
                {
                    // Skip inaccessible files
                }
            }

            await Task.WhenAll(tasks);
        }
        catch (UnauthorizedAccessException)
        {
            // Skip inaccessible directories
        }
        catch (IOException)
        {
            // Skip directories with I/O errors
        }
    }

    private static void UpdateFolderSize(DiskItemEntity folder)
    {
        if (!folder.IsFolder)
            return;

        foreach (var child in folder.Children)
        {
            UpdateFolderSize(child);
        }

        folder.SizeBytes = folder.Children.Sum(c => c.SizeBytes);
    }

    private static void SortChildrenBySize(DiskItemEntity folder)
    {
        folder.Children.Sort((a, b) => b.SizeBytes.CompareTo(a.SizeBytes));

        foreach (var child in folder.Children)
        {
            SortChildrenBySize(child);
        }
    }
}
