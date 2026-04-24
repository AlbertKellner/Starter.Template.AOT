using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.UnitTest.TestHelpers;

public class FakeDiskScannerService : IDiskScannerService
{
    private readonly List<string> _driveNames;
    private readonly bool _returnNullOnFindFolder;

    public FakeDiskScannerService(List<string> driveNames, bool returnNullOnFindFolder = false)
    {
        _driveNames = driveNames;
        _returnNullOnFindFolder = returnNullOnFindFolder;
    }

    public List<string> GetDriveNames() => _driveNames;

    public Task<DiskItemEntity> ScanDriveAsync(string drivePath)
    {
        var root = new DiskItemEntity
        {
            Name = "root",
            FullPath = drivePath,
            Children =
            [
                new DiskItemEntity
                {
                    Name = "TestFolder",
                    FullPath = $"{drivePath}TestFolder",
                    Children =
                    [
                        new DiskItemEntity
                        {
                            Name = "file.txt",
                            Size = 1024,
                            Extension = ".txt",
                            FullPath = $"{drivePath}TestFolder/file.txt"
                        }
                    ]
                }
            ]
        };

        root.UpdateFolderSize();

        return Task.FromResult(root);
    }

    public DiskItemEntity? FindFolder(DiskItemEntity structure, string folderPath)
    {
        if (_returnNullOnFindFolder)
            return null;

        return structure;
    }
}
