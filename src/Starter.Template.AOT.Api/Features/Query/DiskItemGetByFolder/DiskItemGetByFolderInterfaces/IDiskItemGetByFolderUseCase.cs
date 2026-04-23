namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public interface IDiskItemGetByFolderUseCase
{
    Task<DiskItemGetByFolderOutput?> ExecuteAsync(int driveIndex, string folderPath);
}
