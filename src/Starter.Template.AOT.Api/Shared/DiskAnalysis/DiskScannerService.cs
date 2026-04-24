namespace Starter.Template.AOT.Api.Shared.DiskAnalysis;

public class DiskScannerService(ILogger<DiskScannerService> logger) : IDiskScannerService
{
    public List<string> GetDriveNames()
    {
        logger.LogInformation("[DiskScannerService][GetDriveNames] Listar drives disponíveis no sistema");

        var drives = DriveInfo.GetDrives();
        var driveNames = new List<string>();

        foreach (var drive in drives)
        {
            driveNames.Add(drive.Name);
        }

        logger.LogInformation("[DiskScannerService][GetDriveNames] Retornar {Count} drives encontrados", driveNames.Count);

        return driveNames;
    }

    public async Task<DiskItemEntity> ScanDriveAsync(string drivePath)
    {
        logger.LogInformation("[DiskScannerService][ScanDriveAsync] Iniciar escaneamento do caminho {Path}", drivePath);

        var rootFolder = new DiskItemEntity
        {
            Name = "root",
            FullPath = drivePath,
            Children = []
        };

        await ScanDirectoryAsync(rootFolder);

        rootFolder.SortChildrenBySize();

        logger.LogInformation("[DiskScannerService][ScanDriveAsync] Retornar estrutura escaneada com {Count} itens no nível raiz", rootFolder.Children?.Count ?? 0);

        return rootFolder;
    }

    public DiskItemEntity? FindFolder(DiskItemEntity structure, string folderPath)
    {
        logger.LogInformation("[DiskScannerService][FindFolder] Buscar pasta {FolderPath} na estrutura", folderPath);

        var pathSegments = folderPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var result = structure.FindFolder(pathSegments);

        logger.LogInformation("[DiskScannerService][FindFolder] Retornar resultado da busca: {Found}", result is not null ? "encontrada" : "não encontrada");

        return result;
    }

    private async Task ScanDirectoryAsync(DiskItemEntity folder)
    {
        try
        {
            var directoryInfo = new DirectoryInfo(folder.FullPath);

            if (!directoryInfo.Exists)
                return;

            var tasks = new List<Task>();

            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                try
                {
                    var subFolder = new DiskItemEntity
                    {
                        Name = directory.Name,
                        FullPath = directory.FullName,
                        Children = []
                    };

                    folder.Children?.Add(subFolder);

                    tasks.Add(Task.Run(async () => await ScanDirectoryAsync(subFolder)));
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignorar pastas sem permissão de acesso
                }
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                try
                {
                    var fileItem = new DiskItemEntity
                    {
                        Name = file.Name,
                        Size = file.Length,
                        Extension = file.Extension,
                        FullPath = file.FullName
                    };

                    folder.Children?.Add(fileItem);
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignorar arquivos sem permissão de acesso
                }
            }

            await Task.WhenAll(tasks);

            folder.UpdateFolderSize();
        }
        catch (UnauthorizedAccessException)
        {
            // Ignorar diretórios sem permissão de acesso
        }
    }
}
