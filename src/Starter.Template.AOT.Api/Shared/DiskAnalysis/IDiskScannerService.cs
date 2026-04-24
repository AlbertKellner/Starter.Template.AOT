namespace Starter.Template.AOT.Api.Shared.DiskAnalysis;

public interface IDiskScannerService
{
    List<string> GetDriveNames();

    Task<DiskItemEntity> ScanDriveAsync(string drivePath);

    DiskItemEntity? FindFolder(DiskItemEntity structure, string folderPath);
}
