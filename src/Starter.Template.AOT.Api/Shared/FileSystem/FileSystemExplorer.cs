using Starter.Template.AOT.Api.Shared.DiskItem;

namespace Starter.Template.AOT.Api.Shared.FileSystem;

public static class FileSystemExplorer
{
    private const int MaxDepth = 6;

    public static async Task<DiskItemEntity> ScanAsync(string path)
    {
        var root = new DiskItemEntity
        {
            Name = "root",
            FullPath = path,
            Children = []
        };

        await ScanDirectoryAsync(root, 0);

        root.SortChildrenBySize();

        return root;
    }

    private static async Task ScanDirectoryAsync(DiskItemEntity folder, int depth)
    {
        if (depth >= MaxDepth)
            return;

        DirectoryInfo dir;

        try
        {
            dir = new DirectoryInfo(folder.FullPath);
            if (!dir.Exists)
                return;
        }
        catch (IOException)
        {
            return;
        }
        catch (UnauthorizedAccessException)
        {
            return;
        }

        IEnumerable<FileSystemInfo> entries;

        try
        {
            entries = dir.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
        }
        catch (UnauthorizedAccessException)
        {
            return;
        }
        catch (DirectoryNotFoundException)
        {
            return;
        }
        catch (IOException)
        {
            return;
        }

        var tasks = new List<Task>();

        foreach (var entry in entries)
        {
            if (entry is DirectoryInfo subDir)
            {
                var subFolder = new DiskItemEntity
                {
                    Name = subDir.Name,
                    FullPath = subDir.FullName,
                    Children = []
                };

                folder.Children.Add(subFolder);

                var capturedFolder = subFolder;
                var capturedDepth = depth;
                tasks.Add(Task.Run(async () => await ScanDirectoryAsync(capturedFolder, capturedDepth + 1)));
            }
            else if (entry is FileInfo file)
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

                    folder.Children.Add(fileItem);
                }
                catch (FileNotFoundException)
                {
                }
                catch (IOException)
                {
                }
            }
        }

        await Task.WhenAll(tasks);

        folder.UpdateFolderSize();
    }
}
